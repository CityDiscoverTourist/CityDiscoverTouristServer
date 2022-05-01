using AutoMapper;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class CompetitionService: ICompetitionService
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

        //Search(ref listAll, param);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        //var shapedData = _dataShaper.ShapeData(sortedQuests, param.Fields);
        var mappedData = _mapper.Map<IEnumerable<Competition>>(sortedQuests);
        return PageList<Competition>.ToPageList(mappedData, @params.PageNume, @params.PageSize);
    }

    public async Task<Competition> Get(int id)
    {
        var entity = await _competitionRepository.Get(id);

        return _mapper.Map<Competition>(entity);
    }

    public async Task<Competition> CreateAsync(Competition request)
    {
        var entity = _mapper.Map<Competition>(request);
        entity = await _competitionRepository.Add(entity);
        return _mapper.Map<Competition>(entity);
    }

    public async Task<Competition> UpdateAsync(Competition request)
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

    /*private static void Search(ref IQueryable<Competition> entities, QuestParams param)
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