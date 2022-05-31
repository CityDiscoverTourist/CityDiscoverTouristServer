using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class QuestTypeService : BaseService, IQuestTypeService
{
    private readonly IQuestTypeRepository _questTypeRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<QuestType> _sortHelper;
    private readonly IBlobService _blobService;

    public QuestTypeService(IMapper mapper, ISortHelper<QuestType> sortHelper, IQuestTypeRepository questTypeRepository, IBlobService blobService)
    {
        _mapper = mapper;
        _sortHelper = sortHelper;
        _questTypeRepository = questTypeRepository;
        _blobService = blobService;
    }

    public PageList<QuestTypeResponseModel> GetAll(QuestTypeParams @params)
    {
        var listAll = _questTypeRepository.GetAll();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var mappedData = _mapper.Map<IEnumerable<QuestTypeResponseModel>>(sortedQuests);

        return PageList<QuestTypeResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }

    public async Task<QuestTypeResponseModel> Get(int id)
    {
        var entity = await _questTypeRepository.Get(id);
        CheckDataNotNull("QuestType", entity);
        return _mapper.Map<QuestTypeResponseModel>(entity);
    }

    public async Task<QuestTypeResponseModel> CreateAsync(QuestTypeRequestModel request)
    {
        var entity = _mapper.Map<QuestType>(request);
        entity = await _questTypeRepository.Add(entity);

        var imgPath = await _blobService.UploadQuestImgAndReturnImgPathAsync(request.Image, entity.Id);
        entity.ImagePath = imgPath;
        await _questTypeRepository.UpdateFields(entity, r => r.ImagePath!);

        return _mapper.Map<QuestTypeResponseModel>(entity);
    }

    public async Task<QuestTypeResponseModel> UpdateAsync(QuestTypeRequestModel request)
    {
        var imgPath = await _blobService.UploadQuestImgAndReturnImgPathAsync(request.Image, request.Id);

        var entity = _mapper.Map<QuestType>(request);

        entity.ImagePath = imgPath;
        entity = await _questTypeRepository.UpdateFields(entity, r => r.ImagePath!);

        return _mapper.Map<QuestTypeResponseModel>(entity);
    }

    public async Task<QuestTypeResponseModel> DeleteAsync(int id)
    {
        var entity = await _questTypeRepository.Delete(id);
        return _mapper.Map<QuestTypeResponseModel>(entity);
    }

    private static void Search(ref IQueryable<QuestType> entities, QuestTypeParams param)
    {
        if (!entities.Any() || string.IsNullOrWhiteSpace(param.Name) && string.IsNullOrWhiteSpace(param.Status)) return;

        //entities = entities.Where(r => r.Name!.Contains(param.Name!) || r.Status!.Contains(param.Status!));
    }
}