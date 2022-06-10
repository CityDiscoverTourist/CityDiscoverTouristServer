using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Exceptions;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CityDiscoverTourist.Business.IServices.Services;

public class QuestTypeService : BaseService, IQuestTypeService
{
    private readonly IQuestTypeRepository _questTypeRepository;
    private readonly IQuestRepository _questRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<QuestType> _sortHelper;
    private readonly IBlobService _blobService;

    public QuestTypeService(IMapper mapper, ISortHelper<QuestType> sortHelper, IQuestTypeRepository questTypeRepository, IBlobService blobService, IQuestRepository questRepository)
    {
        _mapper = mapper;
        _sortHelper = sortHelper;
        _questTypeRepository = questTypeRepository;
        _blobService = blobService;
        _questRepository = questRepository;
    }

    public PageList<QuestTypeResponseModel> GetAll(QuestTypeParams @params)
    {
        var listAll = _questTypeRepository.GetAll()
            .Include(x => x.Quests)
            .AsNoTracking();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var mappedData = _mapper.Map<IEnumerable<QuestTypeResponseModel>>(sortedQuests);
        return PageList<QuestTypeResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }

    public async Task<QuestTypeResponseModel> Get(int id)
    {
        var entity = await _questTypeRepository.GetByCondition(x => x.Id == id)
            .Include(x => x.Quests)
            .FirstOrDefaultAsync();
        CheckDataNotNull("QuestType", entity!);
        return _mapper.Map<QuestTypeResponseModel>(entity);
    }

    public async Task<QuestTypeResponseModel> CreateAsync(QuestTypeRequestModel request)
    {
        var existValue = _questTypeRepository.GetByCondition(x => request.Name == x.Name).FirstOrDefaultAsync().Result;
        if(existValue!.Name == request.Name) throw new AppException("Quest type already exists");

        var entity = _mapper.Map<QuestType>(request);
        entity = await _questTypeRepository.Add(entity);

        var imgPath = await _blobService.UploadQuestImgAndReturnImgPathAsync(request.Image, entity.Id, "quest-type");
        entity.ImagePath = imgPath;
        await _questTypeRepository.UpdateFields(entity, r => r.ImagePath!);

        return _mapper.Map<QuestTypeResponseModel>(entity);
    }

    public async Task<QuestTypeResponseModel> UpdateAsync(QuestTypeRequestModel request)
    {
        var existValue = _questTypeRepository.GetByCondition(x => request.Name == x.Name).FirstOrDefaultAsync().Result;
        if(existValue!.Name == request.Name) throw new AppException("Quest type already exists");

        var imgPath = await _blobService.UploadQuestImgAndReturnImgPathAsync(request.Image, request.Id, "quest-type");

        var entity = _mapper.Map<QuestType>(request);

        entity.ImagePath = imgPath;
        if (entity.ImagePath == null)
        {
            entity = await _questTypeRepository.NoneUpdateFields(entity, r => r.Id!, r => r.ImagePath!);
            return _mapper.Map<QuestTypeResponseModel>(entity);
        }
        entity = await _questTypeRepository.NoneUpdateFields(entity, r => r.Id!);

        return _mapper.Map<QuestTypeResponseModel>(entity);
    }

    public async Task<QuestTypeResponseModel> DeleteAsync(int id)
    {
        var entity = await _questTypeRepository.Delete(id);
        return _mapper.Map<QuestTypeResponseModel>(entity);
    }

    public async Task<int> CountQuestInQuestType(int questTypeId)
    {
        return _questRepository.GetAll().Count(x => x.QuestTypeId == questTypeId);
    }

    private static void Search(ref IQueryable<QuestType> entities, QuestTypeParams param)
    {
        if (!entities.Any()) return;

        if (param.Name != null)
        {
            entities = entities.Where(x => x.Name!.Contains(param.Name));
        }
    }
}