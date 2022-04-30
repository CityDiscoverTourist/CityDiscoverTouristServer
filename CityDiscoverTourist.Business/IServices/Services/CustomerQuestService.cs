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
    private readonly ICustomerQuestRepository _customerQuestRepository;
    private readonly IQuestItemRepository _taskRepository;
    private readonly IMapper _mapper;
    private const int BaseMultiplier = 150;

    public CustomerQuestService(ICustomerQuestRepository customerQuestRepository, IMapper mapper, IQuestItemRepository taskRepository)
    {
        _customerQuestRepository = customerQuestRepository;
        _mapper = mapper;
        _taskRepository = taskRepository;
    }

    public async Task<CustomerQuestResponseModel> Get(int id)
    {
        var entity = await _customerQuestRepository.Get(id);

        return _mapper.Map<CustomerQuestResponseModel>(entity);
    }

    public Task<CustomerQuestResponseModel> CreateAsync(CustomerQuestRequestModel request)
    {
        throw new NotImplementedException();
    }

    /*public async Task<CustomerQuestResponseModel> CreateAsync(CustomerQuestRequestModel request)
    {
        var numberOfTask = CountTaskInQuest(request.QuestId);
        var entity = _mapper.Map<CustomerQuest>(request);
        var beginPoint = numberOfTask * BaseMultiplier;

        entity.BeginPoint = beginPoint.ToString();
        entity = await _customerQuestRepository.Add(entity);
        return _mapper.Map<CustomerQuestResponseModel>(entity);
    }*/

    public async Task<CustomerQuestResponseModel> UpdateAsync(CustomerQuestRequestModel request)
    {
        var entity = _mapper.Map<CustomerQuest>(request);
        entity = await _customerQuestRepository.Update(entity);
        return _mapper.Map<CustomerQuestResponseModel>(entity);
    }

    public async Task<CustomerQuestResponseModel> DeleteAsync(int id)
    {
        var entity = await _customerQuestRepository.Delete(id);
        return _mapper.Map<CustomerQuestResponseModel>(entity);
    }

    /*private int CountTaskInQuest(Guid questId)
    {
        var listAll = _taskRepository.GetAll();
        var count = listAll.Count(r => r.QuestId.Equals(questId));
        return count;
    }*/
}