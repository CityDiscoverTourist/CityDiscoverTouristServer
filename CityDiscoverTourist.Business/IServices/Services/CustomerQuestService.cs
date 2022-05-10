using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Quest = CityDiscoverTourist.Data.Models.Quest;

namespace CityDiscoverTourist.Business.IServices.Services;

public class CustomerQuestService: BaseService, ICustomerQuestService
{
    private readonly ICustomerQuestRepository _customerQuestRepository;
    private readonly IQuestItemRepository _taskRepository;
    private readonly IMapper _mapper;
    private readonly ICompetitionRepository _competitionRepository;
    private readonly ISortHelper<CustomerQuest> _sortHelper;
    private const int BaseMultiplier = 150;

    public CustomerQuestService(ICustomerQuestRepository customerQuestRepository, IMapper mapper, IQuestItemRepository taskRepository, ISortHelper<CustomerQuest> sortHelper, ICompetitionRepository competitionRepository)
    {
        _customerQuestRepository = customerQuestRepository;
        _mapper = mapper;
        _taskRepository = taskRepository;
        _sortHelper = sortHelper;
        _competitionRepository = competitionRepository;
    }

    public PageList<CustomerQuestResponseModel> GetAll(CustomerQuestParams @params)
    {
        var listAll = _customerQuestRepository.GetAll();

        //Search(ref listAll, param);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        //var shapedData = _dataShaper.ShapeData(sortedQuests, param.Fields);
        var mappedData = _mapper.Map<IEnumerable<CustomerQuestResponseModel>>(sortedQuests);
        return PageList<CustomerQuestResponseModel>.ToPageList(mappedData, @params.PageNume, @params.PageSize);
    }

    public async Task<CustomerQuestResponseModel> Get(int id)
    {
        var entity = await _customerQuestRepository.Get(id);
        CheckDataNotNull("CustomerQuest", entity);
        return _mapper.Map<CustomerQuestResponseModel>(entity);
    }

    public async Task<CustomerQuestResponseModel> CreateAsync(CustomerQuestRequestModel request)
    {
        var s = _competitionRepository.GetAll().FirstOrDefault(x => x.Id == request.CompetitionId);

        var numberOfTask = CountTaskInQuest(s!.QuestId);
        var entity = _mapper.Map<CustomerQuest>(request);
        var beginPoint = numberOfTask * BaseMultiplier;

        entity.BeginPoint = beginPoint.ToString();
        entity = await _customerQuestRepository.Add(entity);
        return _mapper.Map<CustomerQuestResponseModel>(entity);
    }

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

    private int CountTaskInQuest(int questId)
    {
        var listAll = _taskRepository.GetAll();
        var count = listAll.Count(r => r.QuestId.Equals(questId));
        return count;
    }
}