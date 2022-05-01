using AutoMapper;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class AreaService: IAreaService
{
    private readonly IAreaRepository _areaRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<Area> _sortHelper;

    public AreaService(IAreaRepository areaRepository, IMapper mapper, ISortHelper<Area> sortHelper)
    {
        _areaRepository = areaRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }

    public PageList<Area> GetAll(AreaParams @params)
    {
        var listAll = _areaRepository.GetAll();

        //Search(ref listAll, param);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        //var shapedData = _dataShaper.ShapeData(sortedQuests, param.Fields);
        var mappedData = _mapper.Map<IEnumerable<Area>>(sortedQuests);
        return PageList<Area>.ToPageList(mappedData, @params.PageNume, @params.PageSize);
    }

    public async Task<Area> Get(int id)
    {
        var entity = await _areaRepository.Get(id);

        return _mapper.Map<Area>(entity);
    }

    public async Task<Area> CreateAsync(Area request)
    {
        var entity = _mapper.Map<Area>(request);
        entity = await _areaRepository.Add(entity);
        return _mapper.Map<Area>(entity);
    }

    public async Task<Area> UpdateAsync(Area request)
    {
        var entity = _mapper.Map<Area>(request);
        entity = await _areaRepository.Update(entity);
        return _mapper.Map<Area>(entity);
    }

    public async Task<Area> DeleteAsync(int id)
    {
        var entity = await _areaRepository.Delete(id);
        return _mapper.Map<Area>(entity);
    }

    /*private static void Search(ref IQueryable<Area> entities, QuestParams param)
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