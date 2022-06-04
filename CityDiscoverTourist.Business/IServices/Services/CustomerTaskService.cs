using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
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
    private static  GoogleApiSetting? _googleApiSettings;
    private readonly ILocationService _locationService;
    private const int PointWhenHitSuggestion = 150;
    private const int PointWhenWrongAnswer = 100;
    private const float DistanceThreshold = 1000;


    public CustomerTaskService(ICustomerTaskRepository customerTaskRepository, IMapper mapper, ISortHelper<CustomerTask> sortHelper, ICustomerQuestRepository customerQuestRepo, IQuestItemRepository questItemRepo, ILocationRepository locationRepo, GoogleApiSetting? googleApiSettings, ILocationService locationService)
    {
        _customerTaskRepo = customerTaskRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
        _customerQuestRepo = customerQuestRepo;
        _questItemRepo = questItemRepo;
        _locationRepo = locationRepo;
        _googleApiSettings = googleApiSettings;
        _locationService = locationService;
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

    public async Task<CustomerTaskResponseModel> CreateAsync(CustomerTaskRequestModel request)
    {
        var entity = _mapper.Map<CustomerTask>(request);
        entity = await _customerTaskRepo.Add(entity);
        return _mapper.Map<CustomerTaskResponseModel>(entity);
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

        customerTask!.CurrentPoint = currentPoint - PointWhenHitSuggestion;
        customerTask = await _customerTaskRepo.UpdateFields(customerTask, r => r.CurrentPoint);
        return _mapper.Map<CustomerTaskResponseModel>(customerTask);
    }

    public async Task<CustomerTaskResponseModel> DecreasePointWhenWrongAnswer(int customerQuestId)
    {
        var currentPoint = _customerTaskRepo
            .GetByCondition(x => x.CustomerQuestId == customerQuestId)
            .OrderByDescending(x => x.CurrentPoint)
            .LastOrDefaultAsync().Result!.CurrentPoint;

        var customerTask = await _customerTaskRepo.GetByCondition(x => x.CustomerQuestId == customerQuestId)
            .OrderByDescending(x => x.CurrentPoint)
            .LastOrDefaultAsync();

        customerTask!.CurrentPoint = currentPoint - PointWhenWrongAnswer;
        customerTask = await _customerTaskRepo.UpdateFields(customerTask, r => r.CurrentPoint);
        return _mapper.Map<CustomerTaskResponseModel>(customerTask);
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
}