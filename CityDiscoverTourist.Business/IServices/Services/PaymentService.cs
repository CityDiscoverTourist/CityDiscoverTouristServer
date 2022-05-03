using AutoMapper;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class PaymentService: IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<Payment> _sortHelper;

    public PaymentService(IPaymentRepository paymentRepository, IMapper mapper, ISortHelper<Payment> sortHelper)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }

    public PageList<Payment> GetAll(PaymentParams @params)
    {
        var listAll = _paymentRepository.GetAll();

        //Search(ref listAll, param);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        //var shapedData = _dataShaper.ShapeData(sortedQuests, param.Fields);
        var mappedData = _mapper.Map<IEnumerable<Payment>>(sortedQuests);
        return PageList<Payment>.ToPageList(mappedData, @params.PageNume, @params.PageSize);
    }

    public async Task<Payment> Get(int id)
    {
        var entity = await _paymentRepository.Get(id);

        return _mapper.Map<Payment>(entity);
    }

    public async Task<Payment> CreateAsync(Payment request)
    {
        var entity = _mapper.Map<Payment>(request);
        entity = await _paymentRepository.Add(entity);
        return _mapper.Map<Payment>(entity);
    }

    public async Task<Payment> UpdateAsync(Payment request)
    {
        var entity = _mapper.Map<Payment>(request);
        entity = await _paymentRepository.Update(entity);
        return _mapper.Map<Payment>(entity);
    }

    public async Task<Payment> DeleteAsync(int id)
    {
        var entity = await _paymentRepository.Delete(id);
        return _mapper.Map<Payment>(entity);
    }

    /*private static void Search(ref IQueryable<Payment> entities, QuestParams param)
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