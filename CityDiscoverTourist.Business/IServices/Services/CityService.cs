using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Exceptions;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CityDiscoverTourist.Business.IServices.Services;

public class CityService : BaseService, ICityService
{
    private readonly ICityRepository _cityRepository;
    private readonly IAreaRepository _areaRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<City> _sortHelper;

    public CityService(ICityRepository questRepository, IMapper mapper, ISortHelper<City> sortHelper, IAreaRepository areaRepository)
    {
        _cityRepository = questRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
        _areaRepository = areaRepository;
    }

    public PageList<CityResponseModel> GetAll(CityParams @params)
    {
        var listAll = _cityRepository.GetAll().Include(x => x.Areas).AsNoTracking();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var mappedData = _mapper.Map<IEnumerable<CityResponseModel>>(sortedQuests);
        return PageList<CityResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }

    public async Task<CityResponseModel> Get(int id)
    {
        var entity = await _cityRepository.Get(id);
        CheckDataNotNull("City", entity);
        return _mapper.Map<CityResponseModel>(entity);
    }

    public async Task<CityResponseModel> CreateAsync(CityRequestModel request)
    {
        request.Validate();
        var existValue = _cityRepository.GetByCondition(x => request.Name == x.Name).FirstOrDefaultAsync().Result;
        if (existValue != null) throw new AppException("City already exists");

        var entity = _mapper.Map<City>(request);
        entity = await _cityRepository.Add(entity);
        return _mapper.Map<CityResponseModel>(entity);
    }

    public async Task<CityResponseModel> UpdateAsync(CityRequestModel request)
    {
        request.Validate();

        var entity = _mapper.Map<City>(request);
        entity = await _cityRepository.Update(entity);
        return _mapper.Map<CityResponseModel>(entity);
    }

    public async Task<CityResponseModel> DeleteAsync(int id)
    {
        var city = await _cityRepository.Get(id);
        city.Status = CommonStatus.Deleted.ToString();
        await _cityRepository.UpdateFields(city, r => r.Status!);
        return _mapper.Map<CityResponseModel>(city);
    }

    public async Task<CityResponseModel> DisableAsync(int id)
    {
        var city = await _cityRepository.Get(id);
        city.Status = CommonStatus.Inactive.ToString();
        await _cityRepository.UpdateFields(city, r => r.Status!);
        return _mapper.Map<CityResponseModel>(city);
    }

    public async Task<CityResponseModel> EnableAsync(int id)
    {
        var city = await _cityRepository.Get(id);
        city.Status = CommonStatus.Active.ToString();
        await _cityRepository.UpdateFields(city, r => r.Status!);
        return _mapper.Map<CityResponseModel>(city);
    }

    public async Task<CityResponseModel> UpdateStatusForeignKey( int cityId, string status)
    {
        var areaByCityId = _areaRepository.GetByCondition(x => x.CityId == cityId).ToList();
        foreach (var area in areaByCityId)
        {
            area.Status = status;
            await _areaRepository.UpdateFields(area, r => r.Status!);
        }
        return _mapper.Map<CityResponseModel>(Get(cityId).Result);
    }

    private static void Search(ref IQueryable<City> entities, CityParams param)
    {
        if (!entities.Any()) return;

        if (param.Name != null) entities = entities.Where(r => r.Name!.Contains(param.Name));
        if (param.Status != null) entities = entities.Where(x => x.Status == param.Status);
    }
}