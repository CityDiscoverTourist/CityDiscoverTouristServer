using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class CityService: ICityService
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

    public PageList<City> GetAll(CityParams @params)
    {
        var listAll = _cityRepository.GetAll();

        //Search(ref listAll, param);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        //var shapedData = _dataShaper.ShapeData(sortedQuests, param.Fields);
        var mappedData = _mapper.Map<IEnumerable<City>>(sortedQuests);
        return PageList<City>.ToPageList(mappedData, @params.PageNume, @params.PageSize);
    }

    public async Task<City> Get(int id)
    {
        var entity = await _cityRepository.Get(id);

        return _mapper.Map<City>(entity);
    }

    public async Task<City> CreateAsync(City request)
    {
        var entity = _mapper.Map<City>(request);
        entity = await _cityRepository.Add(entity);
        return _mapper.Map<City>(entity);
    }

    public async Task<City> UpdateAsync(City request)
    {
        var entity = _mapper.Map<City>(request);
        entity = await _cityRepository.Update(entity);
        return _mapper.Map<City>(entity);
    }

    public async Task<City> DeleteAsync(int id)
    {
        var entity = await _cityRepository.Delete(id);
        return _mapper.Map<City>(entity);
    }

    /*private static void Search(ref IQueryable<City> entities, QuestParams param)
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