using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class QuestOwnerService: BaseService, IQuestOwnerService
{
    private readonly IQuestOwnerRepository _ownerRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<QuestOwner> _sortHelper;

    public QuestOwnerService(IQuestOwnerRepository ownerRepository, IMapper mapper, ISortHelper<QuestOwner> sortHelper)
    {
        _ownerRepository = ownerRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }

    public PageList<QuestOwnerResponseModel> GetAll(QuestOwnerParams @params)
    {
        var listAll = _ownerRepository.GetAll();

        //Search(ref listAll, param);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        //var shapedData = _dataShaper.ShapeData(sortedQuests, param.Fields);
        var mappedData = _mapper.Map<IEnumerable<QuestOwnerResponseModel>>(sortedQuests);
        return PageList<QuestOwnerResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }

    public async Task<QuestOwnerResponseModel> Get(int id)
    {
        var entity = await _ownerRepository.Get(id);
        CheckDataNotNull("QuestOwner", entity);
        return _mapper.Map<QuestOwnerResponseModel>(entity);
    }

    public async Task<QuestOwnerResponseModel> CreateAsync(QuestOwnerRequestModel request)
    {
        var entity = _mapper.Map<QuestOwner>(request);
        entity = await _ownerRepository.Add(entity);
        return _mapper.Map<QuestOwnerResponseModel>(entity);
    }

    public async Task<QuestOwnerResponseModel> UpdateAsync(QuestOwnerRequestModel request)
    {
        var entity = _mapper.Map<QuestOwner>(request);
        entity = await _ownerRepository.Update(entity);
        return _mapper.Map<QuestOwnerResponseModel>(entity);
    }

    public async Task<QuestOwnerResponseModel> DeleteAsync(int id)
    {
        var entity = await _ownerRepository.Delete(id);
        return _mapper.Map<QuestOwnerResponseModel>(entity);
    }

    /*private static void Search(ref IQueryable<QuestOwner> entities, QuestParams param)
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