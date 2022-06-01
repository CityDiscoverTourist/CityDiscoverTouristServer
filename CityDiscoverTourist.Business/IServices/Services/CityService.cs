using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CityDiscoverTourist.Business.IServices.Services;

public class CityService: BaseService, ICityService
{
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<City> _sortHelper;

    public CityService(ICityRepository questRepository, IMapper mapper, ISortHelper<City> sortHelper)
    {
        _cityRepository = questRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }

    public PageList<CityResponseModel> GetAll(CityParams @params)
    {
        var listAll = _cityRepository.GetAll()
            .Include(x => x.Areas)
            .AsNoTracking();

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
        var entity = _mapper.Map<City>(request);
        entity = await _cityRepository.Add(entity);
        return _mapper.Map<CityResponseModel>(entity);
    }

    public async Task<CityResponseModel> UpdateAsync(CityRequestModel request)
    {
        var entity = _mapper.Map<City>(request);
        entity = await _cityRepository.Update(entity);
        return _mapper.Map<CityResponseModel>(entity);
    }

    public async Task<CityResponseModel> DeleteAsync(int id)
    {
        var entity = await _cityRepository.Delete(id);
        return _mapper.Map<CityResponseModel>(entity);
    }

    private static void Search(ref IQueryable<City> entities, CityParams param)
    {
        if (!entities.Any()) return;

        if(param.Name != null)
        {
            entities = entities.Where(r => r.Name!.Equals((param.Name)));
        }
    }
}