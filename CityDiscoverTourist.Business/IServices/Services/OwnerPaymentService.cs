using AutoMapper;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class OwnerPaymentService: IOwnerPaymentService
{
    private readonly IOwnerPaymentRepository _ownerPaymentRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<OwnerPayment> _sortHelper;

    public OwnerPaymentService(IOwnerPaymentRepository ownerPaymentRepository, IMapper mapper, ISortHelper<OwnerPayment> sortHelper)
    {
        _ownerPaymentRepository = ownerPaymentRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }

    public PageList<OwnerPayment> GetAll(OwnerPaymentParams @params)
    {
        var listAll = _ownerPaymentRepository.GetAll();

        //Search(ref listAll, param);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        //var shapedData = _dataShaper.ShapeData(sortedQuests, param.Fields);
        var mappedData = _mapper.Map<IEnumerable<OwnerPayment>>(sortedQuests);
        return PageList<OwnerPayment>.ToPageList(mappedData, @params.PageNume, @params.PageSize);
    }

    public async Task<OwnerPayment> Get(int id)
    {
        var entity = await _ownerPaymentRepository.Get(id);

        return _mapper.Map<OwnerPayment>(entity);
    }

    public async Task<OwnerPayment> CreateAsync(OwnerPayment request)
    {
        var entity = _mapper.Map<OwnerPayment>(request);
        entity = await _ownerPaymentRepository.Add(entity);
        return _mapper.Map<OwnerPayment>(entity);
    }

    public async Task<OwnerPayment> UpdateAsync(OwnerPayment request)
    {
        var entity = _mapper.Map<OwnerPayment>(request);
        entity = await _ownerPaymentRepository.Update(entity);
        return _mapper.Map<OwnerPayment>(entity);
    }

    public async Task<OwnerPayment> DeleteAsync(int id)
    {
        var entity = await _ownerPaymentRepository.Delete(id);
        return _mapper.Map<OwnerPayment>(entity);
    }

    /*private static void Search(ref IQueryable<OwnerPayment> entities, QuestParams param)
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