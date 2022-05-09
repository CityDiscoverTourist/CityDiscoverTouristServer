using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class PaymentService: BaseService, IPaymentService
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

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        //var shapedData = _dataShaper.ShapeData(sortedQuests, param.Fields);
        var mappedData = _mapper.Map<IEnumerable<Payment>>(sortedQuests);
        return PageList<Payment>.ToPageList(mappedData, @params.PageNume, @params.PageSize);
    }

    public async Task<Payment> Get(int id)
    {
        var entity = await _paymentRepository.Get(id);
        CheckDataNotNull("Payment", entity);
        return _mapper.Map<Payment>(entity);
    }

    public async Task<Payment> CreateAsync(PaymentRequestModel request)
    {
        var entity = _mapper.Map<Payment>(request);
        entity = await _paymentRepository.Add(entity);
        return _mapper.Map<Payment>(entity);
    }

    public async Task<Payment> UpdateAsync(PaymentRequestModel request)
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

    private static void Search(ref IQueryable<Payment> entities, PaymentParams param)
    {
        if (!entities.Any()) return;

        if(param.PaymentMethod != null)
        {
            entities = entities.Where(r => r.PaymentMethod!.Equals(param.PaymentMethod));
        }
        if (param.CustomerQuestId != 0)
        {
            entities = entities.Where(r => r.CustomerQuestId == param.CustomerQuestId);
        }
    }
}