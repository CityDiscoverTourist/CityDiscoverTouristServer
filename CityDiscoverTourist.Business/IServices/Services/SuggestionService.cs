using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class SuggestionService : BaseService, ISuggestionService
{
    private readonly IMapper _mapper;
    private readonly ISortHelper<Suggestion> _sortHelper;
    private readonly ISuggestionRepository _suggestionRepository;

    public SuggestionService(ISuggestionRepository suggestionRepository, IMapper mapper,
        ISortHelper<Suggestion> sortHelper)
    {
        _suggestionRepository = suggestionRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }

    public PageList<SuggestionResponseModel> GetAll(SuggestionParams @params)
    {
        var listAll = _suggestionRepository.GetAll();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var mappedData = _mapper.Map<IEnumerable<SuggestionResponseModel>>(sortedQuests);
        return PageList<SuggestionResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }

    public async Task<SuggestionResponseModel> Get(int id)
    {
        var entity = await _suggestionRepository.Get(id);
        CheckDataNotNull("Suggestion", entity);
        return _mapper.Map<SuggestionResponseModel>(entity);
    }

    public async Task<SuggestionResponseModel> CreateAsync(SuggestionRequestModel request)
    {
        request.Validate();
        var entity = _mapper.Map<Suggestion>(request);
        entity = await _suggestionRepository.Add(entity);
        return _mapper.Map<SuggestionResponseModel>(entity);
    }

    public async Task<SuggestionResponseModel> UpdateAsync(SuggestionRequestModel request)
    {
        request.Validate();
        var entity = _mapper.Map<Suggestion>(request);
        entity = await _suggestionRepository.Update(entity);
        return _mapper.Map<SuggestionResponseModel>(entity);
    }

    public async Task<SuggestionResponseModel> DeleteAsync(int id)
    {
        var entity = await _suggestionRepository.Delete(id);
        return _mapper.Map<SuggestionResponseModel>(entity);
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