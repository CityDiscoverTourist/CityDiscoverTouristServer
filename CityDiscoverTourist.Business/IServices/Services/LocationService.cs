using System.Globalization;
using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.Settings;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Newtonsoft.Json.Linq;

namespace CityDiscoverTourist.Business.IServices.Services;

public class LocationService: BaseService, ILocationService
{
    private readonly ILocationRepository _locationRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<Location> _sortHelper;
    private static  GoogleApiSetting _googleApiSetting;

    public LocationService(ILocationRepository locationRepository, IMapper mapper, ISortHelper<Location> sortHelper, GoogleApiSetting googleApiSetting)
    {
        _locationRepository = locationRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
        _googleApiSetting = googleApiSetting;
    }

    public PageList<LocationResponseModel> GetAll(LocationParams @params)
    {
        var listAll = _locationRepository.GetAll();

        //Search(ref listAll, param);
        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        //var shapedData = _dataShaper.ShapeData(sortedQuests, param.Fields);
        var mappedData = _mapper.Map<IEnumerable<LocationResponseModel>>(sortedQuests);
        return PageList<LocationResponseModel>.ToPageList(mappedData, @params.PageNume, @params.PageSize);
    }

    public async Task<LocationResponseModel> Get(int id)
    {
        var entity = await _locationRepository.Get(id);
        CheckDataNotNull("Location", entity);
        return _mapper.Map<LocationResponseModel>(entity);
    }

    public async Task<LocationResponseModel> CreateAsync(LocationRequestModel request)
    {
        var entity = _mapper.Map<Location>(request);

        var longLat = GetLatLongFromAddress(entity.Address ?? throw new InvalidOperationException());

        entity.Latitude = longLat[0].ToString(CultureInfo.InvariantCulture);
        entity.Longitude = longLat[1].ToString(CultureInfo.InvariantCulture);

        entity = await _locationRepository.Add(entity);
        return _mapper.Map<LocationResponseModel>(entity);
    }

    public async Task<LocationResponseModel> UpdateAsync(LocationRequestModel request)
    {
        var entity = _mapper.Map<Location>(request);
        entity = await _locationRepository.Update(entity);
        return _mapper.Map<LocationResponseModel>(entity);
    }

    public async Task<LocationResponseModel> DeleteAsync(int id)
    {
        var entity = await _locationRepository.Delete(id);
        return _mapper.Map<LocationResponseModel>(entity);
    }

    private static float[] GetLatLongFromAddress(string address)
    {
        var baseUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={address}&key={_googleApiSetting.ApiKey}";

        var client = new HttpClient();
        var response = client.GetAsync(baseUrl).Result;

        var jsonResult = JObject.Parse(response.Content.ReadAsStringAsync().Result);

        var longitude = jsonResult["results"][0]["geometry"]["location"]["lat"].ToString();
        var latitude = jsonResult["results"][0]["geometry"]["location"]["lng"].ToString();

        return new float[] { float.Parse(latitude), float.Parse(longitude) };
    }

    /*private static void Search(ref IQueryable<Location> entities, QuestParams param)
    {
        if (!entities.Any()) return;

        if(param.Name != null)
        {
            entities = entities.Where(r => r.Title!.Contains(param.Name));
        }
        if (param.Description != null)
        {
            entities = entities.Where(r => r.Description!.Contains(param.Description));
        }
        if (param.Status != null)
        {
            entities = entities.Where(r => r.Status!.Contains(param.Status));
        }
    }*/
}