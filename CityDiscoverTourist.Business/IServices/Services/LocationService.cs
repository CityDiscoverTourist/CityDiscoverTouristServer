using System.Globalization;
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

public class LocationService : BaseService, ILocationService
{
    private static  GoongApiSetting? _googleApiSetting;
    private readonly ILocationRepository _locationRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<Location> _sortHelper;

    public LocationService(ILocationRepository locationRepository, IMapper mapper, ISortHelper<Location> sortHelper,
        GoongApiSetting? googleApiSetting)
    {
        _locationRepository = locationRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
        _googleApiSetting = googleApiSetting;
    }

    public PageList<LocationResponseModel> GetAll(LocationParams @params)
    {
        var listAll = _locationRepository.GetAll();

        Search(ref listAll, @params);
        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);

        var mappedData = _mapper.Map<IEnumerable<LocationResponseModel>>(sortedQuests);
        return PageList<LocationResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }

    public async Task<LocationResponseModel> Get(int id)
    {
        var entity = await _locationRepository.Get(id);
        CheckDataNotNull("Location", entity);
        return _mapper.Map<LocationResponseModel>(entity);
    }

    public async Task<LocationResponseModel> CreateAsync(LocationRequestModel request)
    {
        request.Validate();
        var existValue = _locationRepository.GetByCondition(x => request.Name == x.Name).FirstOrDefaultAsync().Result;
        if (existValue != null) throw new AppException("Location with this name already exists");

        var entity = _mapper.Map<Location>(request);

        var longLat = GetLatLongAndPlaceIdFromAddress(entity.Address ?? throw new InvalidOperationException());

        entity.Latitude = longLat[0].ToString(CultureInfo.InvariantCulture);
        entity.Longitude = longLat[1].ToString(CultureInfo.InvariantCulture);

        entity = await _locationRepository.Add(entity);
        return _mapper.Map<LocationResponseModel>(entity);
    }

    public async Task<LocationResponseModel> UpdateAsync(LocationRequestModel request)
    {
        request.Validate();

        var entity = _mapper.Map<Location>(request);
        entity = await _locationRepository.Update(entity);
        return _mapper.Map<LocationResponseModel>(entity);
    }

    public async Task<LocationResponseModel> UpdateAddressAsync(LocationRequestModel request)
    {
        request.Validate();
        var entity = _mapper.Map<Location>(request);
        var longLat = GetLatLongAndPlaceIdFromAddress(entity.Address ?? throw new InvalidOperationException());

        entity.Latitude = longLat[0].ToString(CultureInfo.InvariantCulture);
        entity.Longitude = longLat[1].ToString(CultureInfo.InvariantCulture);

        entity = await _locationRepository.Update(entity);

        return _mapper.Map<LocationResponseModel>(entity);
    }

    public async Task<LocationResponseModel> DeleteAsync(int id)
    {
        var location = await _locationRepository.Get(id);
        location.Status = CommonStatus.Deleted.ToString();
        await _locationRepository.UpdateFields(location, r => r.Status!);
        return _mapper.Map<LocationResponseModel>(location);
    }

    public async Task<LocationResponseModel> DisableAsync(int id)
    {
        var location = await _locationRepository.Get(id);
        location.Status = CommonStatus.Inactive.ToString();
        await _locationRepository.UpdateFields(location, r => r.Status!);
        return _mapper.Map<LocationResponseModel>(location);
    }

    public string[] GetLatLongAndPlaceIdFromAddress(string address)
    {
        var baseUrl = $"https://rsapi.goong.io/Geocode?address={address}&api_key={_googleApiSetting!.ApiKey}";

        var client = new HttpClient();
        var response = client.GetAsync(baseUrl).Result;

        var jsonResult = JObject.Parse(response.Content.ReadAsStringAsync().Result);

        var longitude = jsonResult["results"]![0]!["geometry"]!["location"]!["lng"]!.ToString();
        var latitude = jsonResult["results"]![0]!["geometry"]!["location"]?["lat"]?.ToString();
        var placeId = jsonResult["results"]?[0]?["place_id"]?.ToString();

        return new [] { latitude ?? string.Empty, longitude, placeId ?? string.Empty };
    }

    private static void Search(ref IQueryable<Location> entities, LocationParams param)
    {
        if (!entities.Any()) return;
        param.Validate();

        if (param.Name != null) entities = entities.Where(r => r.Name!.Contains(param.Name));
        if (param.AreaId != 0) entities = entities.Where(r => r.AreaId == param.AreaId);
        if (param.LocationTypeId != 0) entities = entities.Where(r => r.LocationTypeId == param.LocationTypeId);
        if (param.Status != null) entities = entities.Where(x => x.Status == param.Status);
    }
}