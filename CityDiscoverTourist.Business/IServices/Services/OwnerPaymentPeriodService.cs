using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class OwnerPaymentPeriodService: BaseService, IOwnerPaymentPeriodService
{
    private readonly IOwnerPaymentPeriodRepository _ownerPaymentPeriod;
    private readonly IMapper _mapper;
    private readonly ISortHelper<OwnerPaymentPeriod> _sortHelper;

    public OwnerPaymentPeriodService(IOwnerPaymentPeriodRepository ownerPaymentPeriod, IMapper mapper, ISortHelper<OwnerPaymentPeriod> sortHelper)
    {
        _ownerPaymentPeriod = ownerPaymentPeriod;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }

    public PageList<OwnerPaymentPeriodResponseModel> GetAll(OwnerPaymentPeriodParams @params)
    {
        var listAll = _ownerPaymentPeriod.GetAll();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        //var shapedData = _dataShaper.ShapeData(sortedQuests, param.Fields);
        var mappedData = _mapper.Map<IEnumerable<OwnerPaymentPeriodResponseModel>>(sortedQuests);
        return PageList<OwnerPaymentPeriodResponseModel>.ToPageList(mappedData, @params.PageNume, @params.PageSize);
    }

    public async Task<OwnerPaymentPeriodResponseModel> Get(int id)
    {
        var entity = await _ownerPaymentPeriod.Get(id);
        CheckDataNotNull("OwnerPaymentPeriod", entity);
        return _mapper.Map<OwnerPaymentPeriodResponseModel>(entity);
    }

    public async Task<OwnerPaymentPeriodResponseModel> CreateAsync(OwnerPaymentPeriodRm request)
    {
        var entity = _mapper.Map<OwnerPaymentPeriod>(request);
        entity = await _ownerPaymentPeriod.Add(entity);
        return _mapper.Map<OwnerPaymentPeriodResponseModel>(entity);
    }

    public async Task<OwnerPaymentPeriodResponseModel> UpdateAsync(OwnerPaymentPeriodRm request)
    {
        var entity = _mapper.Map<OwnerPaymentPeriod>(request);
        entity = await _ownerPaymentPeriod.Update(entity);
        return _mapper.Map<OwnerPaymentPeriodResponseModel>(entity);
    }

    public async Task<OwnerPaymentPeriodResponseModel> DeleteAsync(int id)
    {
        var entity = await _ownerPaymentPeriod.Delete(id);
        return _mapper.Map<OwnerPaymentPeriodResponseModel>(entity);
    }

    private static void Search(ref IQueryable<OwnerPaymentPeriod> entities, OwnerPaymentPeriodParams param)
    {
        if (!entities.Any()) return;

        if(param.StartDate != null)
        {
            entities = entities.Where(x => x.CreatedDate >= param.StartDate);
        }
        if (param.EndDate != null)
        {
            entities = entities.Where(x => x.CreatedDate <= param.EndDate);
        }
    }
}