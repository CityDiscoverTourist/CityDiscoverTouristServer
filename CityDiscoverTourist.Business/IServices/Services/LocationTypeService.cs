using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class LocationTypeService: BaseService, ILocationTypeService
{
    private readonly ILocationTypeRepository _locationTypeRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<LocationType> _sortHelper;

    public LocationTypeService(ILocationTypeRepository locationTypeRepository, IMapper mapper, ISortHelper<LocationType> sortHelper)
    {
        _locationTypeRepository = locationTypeRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }

    public PageList<LocationTypeResponseModel> GetAll(LocationTypeParams @params)
    {
        var listAll = _locationTypeRepository.GetAll();

        //Search(ref listAll, param);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        //var shapedData = _dataShaper.ShapeData(sortedQuests, param.Fields);
        var mappedData = _mapper.Map<IEnumerable<LocationTypeResponseModel>>(sortedQuests);
        return PageList<LocationTypeResponseModel>.ToPageList(mappedData, @params.PageNume, @params.PageSize);
    }

    public async Task<LocationTypeResponseModel> Get(int id)
    {
        var entity = await _locationTypeRepository.Get(id);
        CheckDataNotNull("LocationType", entity);
        return _mapper.Map<LocationTypeResponseModel>(entity);
    }

    public async Task<LocationTypeResponseModel> CreateAsync(LocationTypeRequestModel request)
    {
        var entity = _mapper.Map<LocationType>(request);
        entity = await _locationTypeRepository.Add(entity);
        return _mapper.Map<LocationTypeResponseModel>(entity);
    }

    public async Task<LocationTypeResponseModel> UpdateAsync(LocationTypeRequestModel request)
    {
        var entity = _mapper.Map<LocationType>(request);
        entity = await _locationTypeRepository.Update(entity);
        return _mapper.Map<LocationTypeResponseModel>(entity);
    }

    public async Task<LocationTypeResponseModel> DeleteAsync(int id)
    {
        var entity = await _locationTypeRepository.Delete(id);
        return _mapper.Map<LocationTypeResponseModel>(entity);
    }

    /*private static void Search(ref IQueryable<LocationType> entities, QuestParams param)
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