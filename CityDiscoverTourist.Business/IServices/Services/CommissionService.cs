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
    private readonly ISortHelper<Commission> _sortHelper;

    public CommissionService(ICommissionRepository questRepository, IMapper mapper, ISortHelper<Commission> sortHelper)
    {
        _commissionService = questRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }

    public PageList<Commission> GetAll(CommissionParams @params)
    {
        var listAll = _commissionService.GetAll();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var mappedData = _mapper.Map<IEnumerable<Commission>>(sortedQuests);
        return PageList<Commission>.ToPageList(mappedData, @params.PageNume, @params.PageSize);
    }

    public async Task<Commission> Get(int id)
    {
        var entity = await _commissionService.Get(id);

        return _mapper.Map<Commission>(entity);
    }

    public async Task<Commission> CreateAsync(CommissionRequestModel request)
    {
        var entity = _mapper.Map<Commission>(request);
        entity = await _commissionService.Add(entity);
        return _mapper.Map<Commission>(entity);
    }

    public async Task<Commission> UpdateAsync(CommissionRequestModel request)
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

    private static void Search(ref IQueryable<Commission> entities, CommissionParams param)
    {
        if (!entities.Any()) return;

        if (param.Percentage != 0)
        {
            entities = entities.Where(x => x.Percentage == param.Percentage);
        }
        if (param.MaxAmount != 0)
        {
            entities = entities.Where(x => x.MaxAmount == param.MaxAmount);
        }

        if (param.MinAmount != 0)
        {
            entities = entities.Where(x => x.MinAmount == param.MinAmount);
        }

        if (param.MaxAmount != 0 && param.MinAmount != 0)
        {
            entities = entities.Where(x => x.MaxAmount == param.MaxAmount && x.MinAmount == param.MinAmount);
        }

        /*if (!entities.Any()) return;

        if (param.Search != null)
        {
            entities = entities.Where(x => x.Percentage.ToString().Contains(param.Search) ||
                                           x.MaxAmount.ToString().Contains(param.Search) ||
                                           x.MinAmount.ToString().Contains(param.Search));
        }*/
    }
}