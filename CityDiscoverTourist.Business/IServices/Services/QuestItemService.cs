using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Exceptions;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace CityDiscoverTourist.Business.IServices.Services;

public class QuestItemService : BaseService, IQuestItemService
{
    private readonly IMapper _mapper;
    private readonly ISortHelper<QuestItem> _sortHelper;
    private readonly IQuestItemRepository _taskRepository;
    private readonly IBlobService _blobService;
    private const string ContainerName = "quest-item";

    public QuestItemService(IQuestItemRepository taskRepository, IMapper mapper, ISortHelper<QuestItem> sortHelper, IBlobService blobService)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
        _blobService = blobService;
    }

    public async Task<PageList<QuestItemResponseModel>> GetAll( TaskParams @params, Language language)
    {
        var listAll = _taskRepository.GetAll().Include(x => x.Suggestions)
            .AsNoTracking();
        Search(ref listAll, @params);
        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var listQuestItem = sortedQuests.ToList();

        var mappedData = _mapper.Map<IEnumerable<QuestItemResponseModel>>(listQuestItem);

        var questItemResponseModels = mappedData as QuestItemResponseModel[] ?? mappedData.ToArray();
        foreach (var item in questItemResponseModels)
        {
            item.ListImages = await _blobService.GetBaseUrl(ContainerName, item.Id);
            item.Content = ConvertLanguage(language, item.Content!);
            item.Description = ConvertLanguage(language, item.Description!);
        }

        return PageList<QuestItemResponseModel>.ToPageList(questItemResponseModels, @params.PageNumber, @params.PageSize);
    }

    public async Task<QuestItemResponseModel> Get(int id, Language language)
    {
        var entity = await _taskRepository.Get(id);
        CheckDataNotNull("QuestItem", entity);

        var mappedData = _mapper.Map<QuestItemResponseModel>(entity);

        mappedData.Content = ConvertLanguage(language, entity.Content!);
        mappedData.Description = ConvertLanguage(language, entity.Description!);
        mappedData.ListImages = await _blobService.GetBaseUrl(ContainerName, mappedData.Id);

        return mappedData;
    }

    public Task<List<string>> GetListImage(int id)
    {
        return _blobService.GetBaseUrl(ContainerName, id);
    }

    public async Task<QuestItemResponseModel> Get(int id)
    {
        var entity = await _taskRepository.Get(id);
        CheckDataNotNull("QuestItem", entity);

        var objContent = JObject.Parse(entity.Content!);
        var content = (string) objContent["vi"]! + " | " + (string) objContent["en"]!;
        var objDescription = JObject.Parse(entity.Description!);
        var description = (string) objDescription["vi"]! + " | " + (string) objDescription["en"]!;

        entity.Content = content;
        entity.Description = description;

        var images = await _blobService.GetBaseUrl(ContainerName, entity.Id);

        var mappedData = _mapper.Map<QuestItemResponseModel>(entity);

        mappedData.ListImages = images;

        return mappedData;
    }

    public async Task<QuestItemResponseModel> CreateAsync(QuestItemRequestModel request)
    {
        request.Validate();
        /*var existValue = _taskRepository
            .GetByCondition(x => x.Content == request.Content || x.Content == ReverseQuestion(request.Content!))
            .FirstOrDefaultAsync().Result;*/
        //if (existValue != null) throw new AppException("Quest item with this name already exists");

        var requestName = GetVietNameseName(request.Content!);

        var existValue = _taskRepository.GetAll();
        foreach (var exist in existValue)
        {
            if (Trim(ConvertLanguage(Language.vi, exist.Content)) == Trim(requestName)
                || ConvertLanguage(Language.vi, exist.Content) == ReverseQuestion(request.Content!.Trim()))
            {
                throw new AppException("Quest item name is exist");
            }
        }


        // set sequence for new quest item
        var questItems = _taskRepository.GetByCondition(x => x.QuestId == request.QuestId).ToList();
        if (questItems.Count == 0)
        {
            request.ItemId = null;
        }
        else
        {
            var lastQuestItemId = questItems.Max(x => x.Id);
            request.ItemId = lastQuestItemId;
        }

        // quest item type 3 is ReverseQuestion
        if (request.QuestItemTypeId == 3) request.Content = ReverseQuestion(request.Content!);

        var entity = _mapper.Map<QuestItem>(request);

        entity.Content = JsonHelper.JsonFormat(request.Content);
        entity.Description = JsonHelper.JsonFormat(request.Description);
        entity.Status = CommonStatus.Active.ToString();

        entity = await _taskRepository.Add(entity);

        // quest item type compare image
        if (request.QuestItemTypeId != 2) return _mapper.Map<QuestItemResponseModel>(entity);

        var imageUrl = await _blobService.UploadQuestItemImgAsync(request.Image, entity.Id, ContainerName);
        entity.AnswerImageUrl = imageUrl;
        await _taskRepository.UpdateFields(entity, x => x.AnswerImageUrl!);

        return _mapper.Map<QuestItemResponseModel>(entity);
    }

    public async Task<QuestItemResponseModel> UpdateAsync(QuestItemRequestModel request)
    {
        request.Validate();

        request.ItemId ??= null;
        var entity = _mapper.Map<QuestItem>(request);

        entity.Content = JsonHelper.JsonFormat(request.Content);
        entity.Description = JsonHelper.JsonFormat(request.Description);

        if(request.QuestItemTypeId == 2)
        {
            var baseUrlImages = await _blobService.GetBaseUrl(ContainerName, entity.Id);

            var expectedList = baseUrlImages.Except(request.ListImages!).ToList();

            foreach (var questItemName in expectedList.Select(image => image.Split("/").Last()))
            {
                await _blobService.DeleteBlogAsync(questItemName, ContainerName, entity.Id);
            }

            var imageUrl = await _blobService.UploadQuestItemImgAsync(request.Image, entity.Id, ContainerName);
            entity.AnswerImageUrl = imageUrl;
            await _taskRepository.UpdateFields(entity, x => x.AnswerImageUrl!);
        }

        entity = await _taskRepository.Update(entity);
        return _mapper.Map<QuestItemResponseModel>(entity);
    }

    public async Task<QuestItemResponseModel> DeleteAsync(int id)
    {
        var entity = await _taskRepository.Get(id);
        entity.Status = CommonStatus.Inactive.ToString();
        await _taskRepository.UpdateFields(entity, r => r.Status!);
        return _mapper.Map<QuestItemResponseModel>(entity);
    }

    public async Task<QuestItemResponseModel> DisableAsync(int id)
    {
        var entity = await _taskRepository.Get(id);
        entity.Status = CommonStatus.Inactive.ToString();
        await _taskRepository.UpdateFields(entity, r => r.Status!);
        return _mapper.Map<QuestItemResponseModel>(entity);
    }

    public async Task<QuestItemResponseModel> EnableAsync(int id)
    {
        var entity = _taskRepository.GetByCondition(x => x.Id == id)
            .Include(data => data.Suggestions).ToList().FirstOrDefault();

        if(entity == null || entity.Suggestions!.Count != 0) return _mapper.Map<QuestItemResponseModel>(entity);

        entity.Status = CommonStatus.Active.ToString();
        await _taskRepository.UpdateFields(entity, r => r.Status!);

        return _mapper.Map<QuestItemResponseModel>(entity);
    }

    public async Task<List<QuestItemResponseModel>> GetByQuestId(int id, Language language)
    {
        var entity = _taskRepository.GetByCondition(x => x.QuestId == id).Include(x => x.Suggestions).ToList();
        CheckDataNotNull("QuestItem", entity);
        var mappedData = _mapper.Map<List<QuestItemResponseModel>>(entity);

        foreach (var item in mappedData)
        {
            var baseUrlImages = await _blobService.GetBaseUrl(ContainerName, item.Id);

            item.ListImages = baseUrlImages;
            item.Content = ConvertLanguage(language, item.Content!);
            item.Description = ConvertLanguage(language, item.Description!);
        }


        return mappedData;
    }

    private static void Search(ref IQueryable<QuestItem> entities, TaskParams param)
    {
        if (!entities.Any()) return;
        if (param.Name != null) entities = entities.Where(x => x.Content!.Contains(param.Name));
        if (param.QuestId != 0) entities = entities.Where(x => x.QuestId == param.QuestId);
        if (param.Status != null) entities = entities.Where(x => x.Status == param.Status);
        if (param.QuestItemTypeId != 0) entities = entities.Where(x => x.QuestItemTypeId == param.QuestItemTypeId);
    }

    private static string ReverseQuestion(string question)
    {
        var reversed = "";
        for (var i = question.Length - 1; i >= 0; i--) reversed += question[i];

        reversed = reversed.Replace(")(", "()");


        var parts = reversed.Split("()");
        // swap the first and last parts
        var first = parts[0];
        var last = parts[parts.Length - 1];
        reversed = last + " () " + first;

        return reversed.Trim();

    }
}