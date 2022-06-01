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
    private const int PointWhenHitSuggestion = 150;
    private const int PointWhenWrongAnswer = 100;

    public CustomerTaskService(ICustomerTaskRepository customerTaskRepository, IMapper mapper, ISortHelper<CustomerTask> sortHelper, ICustomerQuestRepository customerQuestRepo, IQuestItemRepository questItemRepo, ILocationRepository locationRepo, GoogleApiSetting? googleApiSettings)
    {
        _customerTaskRepo = customerTaskRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
        _customerQuestRepo = customerQuestRepo;
        _questItemRepo = questItemRepo;
        _locationRepo = locationRepo;
        _googleApiSettings = googleApiSettings;
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
        var longLat = _locationRepo.Get(locationOfQuestItem).Result.Longitude + " " + _locationRepo.Get(locationOfQuestItem).Result.Latitude;
        return new List<string> { longLat };
    }

    public float DistanceBetweenCustomerLocationAndQuestItem(int customerQuestId, float longitude, float latitude)
    {
        // get from db
        var longLatOfQuestItem = GetLongLatFromCurrentQuestItemOfCustomer(customerQuestId);
        // current user location get from flutter
        //var placeId = _locationService.GetPlaceIdFromLongLat(longitude, latitude);

        return CalculateDistance(longitude + " " + latitude, longLatOfQuestItem.First());
    }

    private static float CalculateDistance(string placeId, string longLat)
    {
        var baseUrl = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={placeId}" +
                      $"&destinations={longLat}&key={_googleApiSettings!.ApiKey2}";
        var client = new HttpClient();
        var response = client.GetAsync(baseUrl).Result;
        var content = response.Content.ReadAsStringAsync().Result;
        var json = JObject.Parse(content);
        var distance = json["rows"][0]["elements"][0]["distance"]["value"];
        return distance!.Value<float>();
    }
}