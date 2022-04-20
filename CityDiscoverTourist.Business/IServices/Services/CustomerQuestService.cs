using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Quest = CityDiscoverTourist.Data.Models.Quest;

namespace CityDiscoverTourist.Business.IServices.Services;

public class CustomerQuestService: ICustomerQuestService
{
    private readonly ICustomerQuestRepository _customerQuestService;
    private readonly IMapper _mapper;

    public CustomerQuestService(ICustomerQuestRepository noteRepository, IMapper mapper)
    {
        _customerQuestService = noteRepository;
        _mapper = mapper;
    }

    public async Task<CustomerQuestResponseModel> Get(int id)
    {
        var entity = await _customerQuestService.Get(id);

        //var shaped = _dataShaper.ShapeData(entity, fields);

        return _mapper.Map<CustomerQuestResponseModel>(entity);
    }

    public async Task<CustomerQuestResponseModel> CreateAsync(CustomerQuestRequestModel request)
    {
        var entity = _mapper.Map<CustomerQuest>(request);
        entity = await _customerQuestService.Add(entity);
        return _mapper.Map<CustomerQuestResponseModel>(entity);
    }

    public async Task<CustomerQuestResponseModel> UpdateAsync(CustomerQuestRequestModel request)
    {
        var entity = _mapper.Map<CustomerQuest>(request);
        entity = await _customerQuestService.Update(entity);
        return _mapper.Map<CustomerQuestResponseModel>(entity);
    }

    public async Task<CustomerQuestResponseModel> DeleteAsync(int id)
    {
        var entity = await _customerQuestService.Delete(id);
        return _mapper.Map<CustomerQuestResponseModel>(entity);
    }
}