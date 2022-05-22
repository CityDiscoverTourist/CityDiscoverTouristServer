using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class CustomerAnswerService: BaseService, ICustomerAnswerService
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

    public PageList<CustomerAnswerResponseModel> GetAll(CustomerAnswerParams @params)
    {
        var listAll = _answerRepository.GetAll();

        //Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var mappedData = _mapper.Map<IEnumerable<CustomerAnswerResponseModel>>(sortedQuests);
        return PageList<CustomerAnswerResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }
    public async Task<CustomerAnswerResponseModel> Get(int id)
    {
        var entity = await _answerRepository.Get(id);

        CheckDataNotNull("CustomerAnswer", entity);

        return _mapper.Map<CustomerAnswerResponseModel>(entity);
    }

    public async Task<CustomerAnswerResponseModel> CreateAsync(CustomerAnswerRequestModel request)
    {
        var entity = _mapper.Map<CustomerAnswer>(request);
        entity = await _answerRepository.Add(entity);
        return _mapper.Map<CustomerAnswerResponseModel>(entity);
    }

    public async Task<CustomerAnswerResponseModel> UpdateAsync(CustomerAnswerRequestModel request)
    {
        var entity = _mapper.Map<CustomerAnswer>(request);
        entity = await _answerRepository.Update(entity);
        return _mapper.Map<CustomerAnswerResponseModel>(entity);
    }

    public async Task<CustomerAnswerResponseModel> DeleteAsync(int id)
    {
        var entity = await _answerRepository.Delete(id);
        return _mapper.Map<CustomerAnswerResponseModel>(entity);
    }
}