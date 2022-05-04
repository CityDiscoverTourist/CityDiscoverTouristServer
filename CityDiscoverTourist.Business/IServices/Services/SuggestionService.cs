using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class SuggestionService: ISuggestionService
{
    private readonly ISuggestionRepository _suggestionRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<Suggestion> _sortHelper;

    public SuggestionService(ISuggestionRepository suggestionRepository, IMapper mapper, ISortHelper<Suggestion> sortHelper)
    {
        _suggestionRepository = suggestionRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }

    public PageList<Suggestion> GetAll(SuggestionParams @params)
    {
        var listAll = _suggestionRepository.GetAll();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var mappedData = _mapper.Map<IEnumerable<Suggestion>>(sortedQuests);
        return PageList<Suggestion>.ToPageList(mappedData, @params.PageNume, @params.PageSize);
    }

    public async Task<Suggestion> Get(int id)
    {
        var entity = await _suggestionRepository.Get(id);

        return _mapper.Map<Suggestion>(entity);
    }

    public async Task<Suggestion> CreateAsync(SuggestionRequestModel request)
    {
        var entity = _mapper.Map<Suggestion>(request);
        entity = await _suggestionRepository.Add(entity);
        return _mapper.Map<Suggestion>(entity);
    }

    public async Task<Suggestion> UpdateAsync(SuggestionRequestModel request)
    {
        var entity = _mapper.Map<Suggestion>(request);
        entity = await _suggestionRepository.Update(entity);
        return _mapper.Map<Suggestion>(entity);
    }

    public async Task<Suggestion> DeleteAsync(int id)
    {
        var entity = await _suggestionRepository.Delete(id);
        return _mapper.Map<Suggestion>(entity);
    }

    private static void Search(ref IQueryable<Suggestion> entities, SuggestionParams param)
    {
        if (!entities.Any()) return;

        /*if (param.CityId != 0)
        {
            entities = entities.Where(x => x.CityId == param.CityId);
        }*/
    }
}