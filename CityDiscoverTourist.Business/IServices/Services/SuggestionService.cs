using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Newtonsoft.Json.Linq;

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

    public PageList<SuggestionResponseModel> GetAll(SuggestionParams @params, Language language)
    {
        var listAll = _suggestionRepository.GetAll();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var mappedData = _mapper.Map<IEnumerable<SuggestionResponseModel>>(sortedQuests);

        var suggestionResponseModels = mappedData as SuggestionResponseModel[] ?? mappedData.ToArray();

        foreach (var suggestion in suggestionResponseModels)
        {
            suggestion.Content = ConvertLanguage(language, suggestion.Content!);
        }

        return PageList<SuggestionResponseModel>.ToPageList(suggestionResponseModels, @params.PageNumber, @params.PageSize);
    }

    public async Task<SuggestionResponseModel> Get(int id)
    {
        var entity = await _suggestionRepository.Get(id);
        CheckDataNotNull("LocationType", entity);

        var objTitle = JObject.Parse(entity.Content!);
        var title = (string) objTitle["vi"]! + " | " + (string) objTitle["en"]!;

        entity.Content = title;

        return _mapper.Map<SuggestionResponseModel>(entity);
    }

    public async Task<SuggestionResponseModel> Get(int id, Language language)
    {
        var entity = await _suggestionRepository.Get(id);
        CheckDataNotNull("Suggestion", entity);

        entity.Content = ConvertLanguage(language, entity.Content);

        return _mapper.Map<SuggestionResponseModel>(entity);
    }

    public async Task<SuggestionResponseModel> CreateAsync(SuggestionRequestModel request)
    {
        request.Validate();
        var entity = _mapper.Map<Suggestion>(request);

        entity.Content = JsonHelper.JsonFormat(request.Content);

        entity = await _suggestionRepository.Add(entity);
        return _mapper.Map<SuggestionResponseModel>(entity);
    }

    public async Task<SuggestionResponseModel> UpdateAsync(SuggestionRequestModel request)
    {
        request.Validate();
        var entity = _mapper.Map<Suggestion>(request);

        entity.Content = JsonHelper.JsonFormat(request.Content);

        entity = await _suggestionRepository.Update(entity);
        return _mapper.Map<SuggestionResponseModel>(entity);
    }

    public async Task<SuggestionResponseModel> DeleteAsync(int id)
    {
        var entity = await _suggestionRepository.Get(id);
        entity.Status = CommonStatus.Inactive.ToString();
        await _suggestionRepository.UpdateFields(entity, r => r.Status!);
        return _mapper.Map<SuggestionResponseModel>(entity);
    }

    public async Task<SuggestionResponseModel> DisableAsync(int id)
    {
        var entity = await _suggestionRepository.Get(id);
        entity.Status = CommonStatus.Inactive.ToString();
        await _suggestionRepository.UpdateFields(entity, r => r.Status!);
        return _mapper.Map<SuggestionResponseModel>(entity);
    }

    public async Task<SuggestionResponseModel> EnableAsync(int id)
    {
        var entity = await _suggestionRepository.Get(id);
        entity.Status = CommonStatus.Active.ToString();
        await _suggestionRepository.UpdateFields(entity, r => r.Status!);
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