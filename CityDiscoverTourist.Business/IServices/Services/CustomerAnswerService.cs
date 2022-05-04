using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class CustomerAnswerService: ICustomerAnswerService
{
    private readonly ICustomerAnswerRepository _answerRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<CustomerAnswer> _sortHelper;


    public CustomerAnswerService(ICustomerAnswerRepository answerRepository, IMapper mapper, ISortHelper<CustomerAnswer> sortHelper)
    {
        _answerRepository = answerRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }

    public PageList<CustomerAnswer> GetAll(CustomerAnswerParams @params)
    {
        var listAll = _answerRepository.GetAll();

        //Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var mappedData = _mapper.Map<IEnumerable<CustomerAnswer>>(sortedQuests);
        return PageList<CustomerAnswer>.ToPageList(mappedData, @params.PageNume, @params.PageSize);
    }
    public async Task<CustomerAnswer> Get(int id)
    {
        var entity = await _answerRepository.Get(id);

        //var shaped = _dataShaper.ShapeData(entity, fields);

        return _mapper.Map<CustomerAnswer>(entity);
    }

    public async Task<CustomerAnswer> CreateAsync(CustomerAnswerRequetModel request)
    {
        var entity = _mapper.Map<CustomerAnswer>(request);
        entity = await _answerRepository.Add(entity);
        return _mapper.Map<CustomerAnswer>(entity);
    }

    public async Task<CustomerAnswer> UpdateAsync(CustomerAnswerRequetModel request)
    {
        var entity = _mapper.Map<CustomerAnswer>(request);
        entity = await _answerRepository.Update(entity);
        return _mapper.Map<CustomerAnswer>(entity);
    }

    public async Task<CustomerAnswer> DeleteAsync(int id)
    {
        var entity = await _answerRepository.Delete(id);
        return _mapper.Map<CustomerAnswer>(entity);
    }
}