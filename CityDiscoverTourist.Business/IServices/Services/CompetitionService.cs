using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class CompetitionService: BaseService, ICompetitionService
{
    private readonly ICompetitionRepository _competitionRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<Competition> _sortHelper;

    public CompetitionService(ICompetitionRepository competitionRepository, IMapper mapper, ISortHelper<Competition> sortHelper)
    {
        _competitionRepository = competitionRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }

    public PageList<Competition> GetAll(CompetitionParams @params)
    {
        var listAll = _competitionRepository.GetAll();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var mappedData = _mapper.Map<IEnumerable<Competition>>(sortedQuests);
        return PageList<Competition>.ToPageList(mappedData, @params.PageNume, @params.PageSize);
    }

    public async Task<Competition> Get(int id)
    {
        var entity = await _competitionRepository.Get(id);
        CheckDataNotNull("Competition", entity);
        return _mapper.Map<Competition>(entity);
    }

    public async Task<Competition> CreateAsync(CompetitionRequestModel request)
    {
        var entity = _mapper.Map<Competition>(request);
        entity = await _competitionRepository.Add(entity);
        return _mapper.Map<Competition>(entity);
    }

    public async Task<Competition> UpdateAsync(CompetitionRequestModel request)
    {
        var entity = _mapper.Map<Competition>(request);
        entity = await _competitionRepository.Update(entity);
        return _mapper.Map<Competition>(entity);
    }

    public async Task<Competition> DeleteAsync(int id)
    {
        var entity = await _competitionRepository.Delete(id);
        return _mapper.Map<Competition>(entity);
    }

    private static void Search(ref IQueryable<Competition> entities, CompetitionParams param)
    {
        if (!entities.Any()) return;

        if(param.CompetitionCode != null)
        {
            entities = entities.Where(r => r.CompetitionCode!.Equals(param.CompetitionCode));
        }
        if (param.QuestId != 0)
        {
            entities = entities.Where(r => r.QuestId == param.QuestId);
        }
    }
}