using System.Linq;
using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Exceptions;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.Settings;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace CityDiscoverTourist.Business.IServices.Services;

public class CustomerTaskService: BaseService, ICustomerTaskService
{
    private readonly ICustomerTaskRepository _customerTaskRepo;
    private readonly IMapper _mapper;
    private readonly ISortHelper<CustomerTask> _sortHelper;
    private readonly ICustomerQuestRepository _customerQuestRepo;
    private readonly IQuestItemRepository _questItemRepo;
    private readonly ILocationRepository _locationRepo;
    private static  GoongApiSetting? _googleApiSettings;
    private readonly ILocationService _locationService;
    private readonly ICustomerAnswerService _customerAnswerService;
    private const int PointWhenHitSuggestion = 150;
    private const int PointWhenWrongAnswer = 100;
    private const float DistanceThreshold = 1000;


    public CustomerTaskService(ICustomerTaskRepository customerTaskRepository, IMapper mapper, ISortHelper<CustomerTask> sortHelper, ICustomerQuestRepository customerQuestRepo, IQuestItemRepository questItemRepo, ILocationRepository locationRepo, GoongApiSetting? googleApiSettings, ILocationService locationService, ICustomerAnswerService customerAnswerService)
    {
        _customerTaskRepo = customerTaskRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
        _customerQuestRepo = customerQuestRepo;
        _questItemRepo = questItemRepo;
        _locationRepo = locationRepo;
        _googleApiSettings = googleApiSettings;
        _locationService = locationService;
        _customerAnswerService = customerAnswerService;
    }

    public PageList<CustomerTaskResponseModel> GetAll(CustomerTaskParams @params)
    {
        var listAll = _customerTaskRepo.GetAll();

        //Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var mappedData = _mapper.Map<IEnumerable<CustomerTaskResponseModel>>(sortedQuests);
        return PageList<CustomerTaskResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }
    public async Task<CustomerTaskResponseModel> Get(int id)
    {
        var entity = await _customerTaskRepo.Get(id);
        CheckDataNotNull("CustomerTask", entity);
        return _mapper.Map<CustomerTaskResponseModel>(entity);
    }

    public async Task<CustomerTaskResponseModel> CustomerStartQuest(CustomerTaskRequestModel request, int questId)
    {
        //insert customer task for first time customer join the quest
        var entity = _mapper.Map<CustomerTask>(request);

        entity.CurrentPoint = Convert.ToSingle(GetBeginPointsAsync(entity.CustomerQuestId));
        entity.IsFinished = false;
        entity.QuestItemId = GetFirstQuestItemIdOfQuest(questId);

        entity = await _customerTaskRepo.Add(entity);
        return _mapper.Map<CustomerTaskResponseModel>(entity);
    }

    public async Task<CustomerTaskResponseModel> MoveCustomerToNextTask(int questId, int customerQuestId)
    {
        // get last quest item customer has done
        var lastQuestItemCustomerFinished = _customerTaskRepo.GetAll()
            .Where(x => x.CustomerQuestId == customerQuestId && x.IsFinished == true)
            .AsEnumerable()
            .LastOrDefault();

        var questItems = _questItemRepo.GetByCondition(x => x.QuestId == questId).ToList();

        // get next quest item
        foreach (var questItem in questItems)
        {
            if (lastQuestItemCustomerFinished!.QuestItemId != questItem.Id) continue;
            //move to next quest item
            var nextQuestItem = questItems.FirstOrDefault(x => x.ItemId == questItem.Id);
            var customerTask = new CustomerTaskResponseModel
            {
                CustomerQuestId = customerQuestId,
                QuestItemId = nextQuestItem!.Id,
                IsFinished = false,
                CurrentPoint = lastQuestItemCustomerFinished.CurrentPoint,
                Status = "Progress"
            };
            await _customerTaskRepo.Add(_mapper.Map<CustomerTask>(customerTask));
        }
        return await Get(customerQuestId);
    }

    public async Task<CustomerTaskResponseModel> UpdateAsync(CustomerTaskRequestModel request)
    {
        var entity = _mapper.Map<CustomerTask>(request);
        entity = await _customerTaskRepo.Update(entity);
        return _mapper.Map<CustomerTaskResponseModel>(entity);
    }

    public async Task<CustomerTaskResponseModel> DeleteAsync(int id)
    {
        var entity = await _customerTaskRepo.Delete(id);
        return _mapper.Map<CustomerTaskResponseModel>(entity);
    }

    public async Task<CustomerTaskResponseModel> UpdateCurrentPointAsync(int id, float currentPoint)
    {
        var customerTask = await _customerTaskRepo.Get(id);
        customerTask.CurrentPoint = currentPoint;
        customerTask = await _customerTaskRepo.UpdateFields(customerTask, r => r.CurrentPoint);
        return _mapper.Map<CustomerTaskResponseModel>(customerTask);
    }

    public async Task<CustomerTaskResponseModel> UpdateStatusAsync(int id, string status)
    {
        var customerTask = await _customerTaskRepo.Get(id);
        customerTask.Status = status;
        customerTask = await _customerTaskRepo.UpdateFields(customerTask, r => r.Status ?? string.Empty);
        return _mapper.Map<CustomerTaskResponseModel>(customerTask);
    }

    public string GetBeginPointsAsync(int customerQuestId)
    {
        return _customerQuestRepo.Get(customerQuestId).Result.BeginPoint!;
    }

    public async Task<CustomerTaskResponseModel> DecreasePointWhenHitSuggestion(int customerQuestId)
    {
        var currentPoint = _customerTaskRepo
            .GetByCondition(x => x.CustomerQuestId == customerQuestId)
            .OrderByDescending(x => x.CurrentPoint)
            .LastOrDefaultAsync().Result!.CurrentPoint;

        var customerTask = await _customerTaskRepo.GetByCondition(x => x.CustomerQuestId == customerQuestId)
            .OrderByDescending(x => x.CurrentPoint)
            .LastOrDefaultAsync();

        if (customerTask!.CountSuggestion >=3) throw new AppException("You have already hit 3 suggestions");

        customerTask!.CurrentPoint = currentPoint - PointWhenHitSuggestion;
        customerTask.CountSuggestion++;

        customerTask = await _customerTaskRepo.UpdateFields(customerTask, r => r.CurrentPoint,
            r => r.CountSuggestion);

        return _mapper.Map<CustomerTaskResponseModel>(customerTask);
    }

    public async Task<CustomerTaskResponseModel> CheckCustomerAnswer(int customerQuestId, string customerReply, int questItemId)
    {
        var isCustomerReplyCorrect = true;
        var isCustomerWrongAnswer = true;

        var currentPoint = _customerTaskRepo
            .GetByCondition(x => x.CustomerQuestId == customerQuestId)
            .OrderByDescending(x => x.CurrentPoint)
            .LastOrDefaultAsync().Result!.CurrentPoint;

        //get current quest item of customer
        var customerTask = await _customerTaskRepo.GetByCondition(x => x.CustomerQuestId == customerQuestId)
            .Where(x => x.QuestItemId == questItemId)
            .OrderByDescending(x => x.CurrentPoint)
            .LastOrDefaultAsync();

        //compare with correct answer
        var correctAnswer = await _questItemRepo.Get(customerTask!.QuestItemId);
        if (!correctAnswer.RightAnswer!.ToLower().Equals(customerReply.ToLower()))
        {
            // count number of customer answer wrong
            // throw when it >= 3
            // show the suggestion
            // not decrease point
            // code here:

            // if count number of customer answer wrong >= 5
            // show right answer move to next quest item
            // code here:
            if (customerTask.CountWrongAnswer >= 3)
            {
                customerTask.CountWrongAnswer++;
                customerTask.CurrentPoint = currentPoint - PointWhenWrongAnswer;
                await _customerTaskRepo.UpdateFields(customerTask, r => r.CurrentPoint,
                    r => r.CountWrongAnswer);
                if (customerTask.CountWrongAnswer >= 5)
                {
                    throw new AppException("You have already hit 5 wrong answers");
                }
            }

            customerTask.CurrentPoint = currentPoint - PointWhenWrongAnswer;
            customerTask.CountWrongAnswer++;
            customerTask = await _customerTaskRepo.UpdateFields(customerTask, r => r.CurrentPoint,
                r => r.CountWrongAnswer);
            isCustomerReplyCorrect = false;
        }
        else
        {
            // correct answer
            customerTask.Status = "Finished";
            customerTask.IsFinished = true;
            await _customerTaskRepo.UpdateFields(customerTask, r => r.Status!, r => r.IsFinished);
        }

        //save customer answer for each time customer answer wrong/true
        if(!isCustomerReplyCorrect)
            await SaveCustomerAnswer(customerTask, customerReply, NoteCustomerAnswer.WrongAnswer);
        else
            await SaveCustomerAnswer(customerTask, customerReply, NoteCustomerAnswer.CorrectAnswer);

        return _mapper.Map<CustomerTaskResponseModel>(customerTask);
    }

    private async Task SaveCustomerAnswer(CustomerTask customerTask, string customerReply, NoteCustomerAnswer note)
    {
        var customerAnswer = new CustomerAnswerRequestModel
        {
            CustomerTaskId = customerTask.Id,
            QuestItemId = customerTask.QuestItemId,
            Note = note.ToString(),
            CustomerReply = customerReply
        };
        var customerAnswerResponse = await _customerAnswerService.CreateAsync(customerAnswer);
    }

    public Task<bool> IsLastQuestItem(int customerQuestId)
    {
        var numOfItemCustomerHasDone = _customerTaskRepo
            .GetByCondition(x => x.CustomerQuestId == customerQuestId).Count();
        var numOfItemInQuest = _questItemRepo
            .GetByCondition(x => x.QuestId == _customerQuestRepo.Get(customerQuestId).Result.QuestId).Count();
        return Task.FromResult(numOfItemCustomerHasDone == numOfItemInQuest);
    }

    public float GetLastPoint(int customerQuestId)
    {
        return _customerTaskRepo
            .GetByCondition(x => x.CustomerQuestId == customerQuestId)
            .OrderByDescending(x => x.CurrentPoint)
            .LastOrDefaultAsync().Result!.CurrentPoint;
    }

    public IEnumerable<string> GetLongLatFromCurrentQuestItemOfCustomer(int customerQuestId)
    {
        //get long lat of current quest item customer prepare to do
        var itemId = _customerTaskRepo
            .GetByCondition(x => x.CustomerQuestId == customerQuestId)
            .OrderByDescending(x => x.CurrentPoint)
            .LastOrDefaultAsync().Result!.QuestItemId;

        var locationOfQuestItem = _questItemRepo.Get(itemId).Result.LocationId;
        var latLong = _locationRepo.Get(locationOfQuestItem).Result.Latitude + "," + _locationRepo.Get(locationOfQuestItem).Result.Longitude;
        return new List<string> { latLong };
    }

    public bool IsCustomerAtQuestItemLocation(int customerQuestId, float latitude, float longitude)
    {
        // get from db
        var longLatOfQuestItem = GetLongLatFromCurrentQuestItemOfCustomer(customerQuestId);

        var distance = CalculateDistanceBetweenCustomerLocationAndQuestItem(longLatOfQuestItem.First(), latitude + "," + longitude);
        return distance < DistanceThreshold;
    }

    private static float CalculateDistanceBetweenCustomerLocationAndQuestItem(string latLongFromLocation, string latLongFromUserDevice)
    {
        var baseUrl = $"https://rsapi.goong.io/DistanceMatrix?origins={latLongFromLocation}" +
                      $"&destinations={latLongFromUserDevice}&vehicle=car&api_key={_googleApiSettings!.ApiKey}";
        var client = new HttpClient();
        var response = client.GetAsync(baseUrl).Result;
        var content = response.Content.ReadAsStringAsync().Result;
        var json = JObject.Parse(content);
        var distance = json["rows"][0]["elements"][0]["distance"]["value"];
        return distance!.Value<float>();
    }

    private int GetFirstQuestItemIdOfQuest(int questId)
    {
        var questItems = _questItemRepo.GetByCondition(x => x.QuestId == questId);
        return (from questItem in questItems.ToList() where questItem.ItemId == null select questItem.Id).FirstOrDefault();
    }
}