using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class OwnerPaymentService: BaseService, IOwnerPaymentService
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

    public PageList<OwnerPaymentResponseModel> GetAll(OwnerPaymentParams @params)
    {
        var listAll = _ownerPaymentRepository.GetAll();

        //Search(ref listAll, param);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        //var shapedData = _dataShaper.ShapeData(sortedQuests, param.Fields);
        var mappedData = _mapper.Map<IEnumerable<OwnerPaymentResponseModel>>(sortedQuests);
        return PageList<OwnerPaymentResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }

    public async Task<OwnerPaymentResponseModel> Get(int id)
    {
        var entity = await _ownerPaymentRepository.Get(id);
        CheckDataNotNull("OwnerPayment", entity);
        return _mapper.Map<OwnerPaymentResponseModel>(entity);
    }

    public async Task<OwnerPaymentResponseModel> CreateAsync(OwnerPaymentRequestModel request)
    {
        var entity = _mapper.Map<OwnerPayment>(request);
        entity = await _ownerPaymentRepository.Add(entity);
        return _mapper.Map<OwnerPaymentResponseModel>(entity);
    }

    public async Task<OwnerPaymentResponseModel> UpdateAsync(OwnerPaymentRequestModel request)
    {
        var entity = _mapper.Map<OwnerPayment>(request);
        entity = await _ownerPaymentRepository.Update(entity);
        return _mapper.Map<OwnerPaymentResponseModel>(entity);
    }

    public async Task<OwnerPaymentResponseModel> DeleteAsync(int id)
    {
        var entity = await _ownerPaymentRepository.Delete(id);
        return _mapper.Map<OwnerPaymentResponseModel>(entity);
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