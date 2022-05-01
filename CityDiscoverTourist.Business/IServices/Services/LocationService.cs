using AutoMapper;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class LocationService: ILocationService
{
    private readonly ILocationRepository _locationRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<Location> _sortHelper;

    public LocationService(ILocationRepository locationRepository, IMapper mapper, ISortHelper<Location> sortHelper)
    {
        _locationRepository = locationRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }

    public PageList<Location> GetAll(LocationParams @params)
    {
        var listAll = _locationRepository.GetAll();

        //Search(ref listAll, param);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        //var shapedData = _dataShaper.ShapeData(sortedQuests, param.Fields);
        var mappedData = _mapper.Map<IEnumerable<Location>>(sortedQuests);
        return PageList<Location>.ToPageList(mappedData, @params.PageNume, @params.PageSize);
    }

    public async Task<Location> Get(int id)
    {
        var entity = await _locationRepository.Get(id);

        return _mapper.Map<Location>(entity);
    }

    public async Task<Location> CreateAsync(Location request)
    {
        var entity = _mapper.Map<Location>(request);
        entity = await _locationRepository.Add(entity);
        return _mapper.Map<Location>(entity);
    }

    public async Task<Location> UpdateAsync(Location request)
    {
        var entity = _mapper.Map<Location>(request);
        entity = await _locationRepository.Update(entity);
        return _mapper.Map<Location>(entity);
    }

    public async Task<Location> DeleteAsync(int id)
    {
        var entity = await _locationRepository.Delete(id);
        return _mapper.Map<Location>(entity);
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