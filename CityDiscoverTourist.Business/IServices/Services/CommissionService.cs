using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class CommissionService: ICommissionService
{
    private readonly ICommissionRepository _commissionService;
    private readonly IMapper _mapper;

    public CommissionService(ICommissionRepository questRepository, IMapper mapper)
    {
        _commissionService = questRepository;
        _mapper = mapper;
    }


    /*public PageList<Commission> GetAll(Commission param)
    {
        var listAll = _commissionService.GetAll();

        //Search(ref listAll, param);

        var sortedQuests = _sortHelper.ApplySort(listAll, param.OrderBy);
        //var shapedData = _dataShaper.ShapeData(sortedQuests, param.Fields);
        var mappedData = _mapper.Map<IEnumerable<Commission>>(sortedQuests);
        return PageList<Commission>.ToPageList(mappedData, param.PageNume, param.PageSize);
    }*/

    public PageList<Commission> GetAll(Commission @params)
    {
        throw new NotImplementedException();
    }

    public async Task<Commission> Get(int id)
    {
        var entity = await _commissionService.Get(id);

        //var shaped = _dataShaper.ShapeData(entity, fields);

        return _mapper.Map<Commission>(entity);
    }

    public async Task<Commission> CreateAsync(Commission request)
    {
        var entity = _mapper.Map<Commission>(request);
        entity = await _commissionService.Add(entity);
        return _mapper.Map<Commission>(entity);
    }

    public async Task<Commission> UpdateAsync(Commission request)
    {
        var entity = _mapper.Map<Commission>(request);
        entity = await _commissionService.Update(entity);
        return _mapper.Map<Commission>(entity);
    }

    public async Task<Commission> DeleteAsync(int id)
    {
        var entity = await _commissionService.Delete(id);
        return _mapper.Map<Commission>(entity);
    }


    /*private static void Search(ref IQueryable<Quest> entities, QuestParams param)
    {
        if (!entities.Any() || string.IsNullOrWhiteSpace(param.Name) && string.IsNullOrWhiteSpace(param.Status)) return;

        entities = entities.Where(r => r.Status!.Contains(param.Status!));


    }*/
    /*private static void Search(ref IQueryable<Commission> entities, QuestParams param)
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