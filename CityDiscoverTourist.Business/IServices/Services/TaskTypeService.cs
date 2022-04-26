using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class TaskTypeService : ITaskTypeService
{
    private readonly ITaskTypeRepository _taskTypeRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<TaskType> _sortHelper;

    public TaskTypeService(ITaskTypeRepository taskTypeRepository, IMapper mapper, ISortHelper<TaskType> sortHelper)
    {
        _taskTypeRepository = taskTypeRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }

    public PageList<TaskTypeResponseModel> GetAll(TaskTypeParams @params)
    {
        var listAll = _taskTypeRepository.GetAll();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var mappedData = _mapper.Map<IEnumerable<TaskTypeResponseModel>>(sortedQuests);
        return PageList<TaskTypeResponseModel>.ToPageList(mappedData, @params.PageNume, @params.PageSize);
    }

    public async Task<TaskTypeResponseModel> Get(int id, string? fields)
    {
        var entity = await _taskTypeRepository.Get(id);

        return _mapper.Map<TaskTypeResponseModel>(entity);
    }

    public async Task<TaskTypeResponseModel> CreateAsync(TaskTypeRequestModel request)
    {
        var entity = _mapper.Map<TaskType>(request);
        entity = await _taskTypeRepository.Add(entity);
        return _mapper.Map<TaskTypeResponseModel>(entity);
    }

    public async Task<TaskTypeResponseModel> UpdateAsync(TaskTypeRequestModel request)
    {
        var entity = _mapper.Map<TaskType>(request);
        entity = await _taskTypeRepository.Update(entity);
        return _mapper.Map<TaskTypeResponseModel>(entity);
    }

    public async Task<TaskTypeResponseModel> DeleteAsync(int id)
    {
        var entity = await _taskTypeRepository.Delete(id);
        return _mapper.Map<TaskTypeResponseModel>(entity);
    }

    private static void Search(ref IQueryable<TaskType> entities, TaskTypeParams param)
    {
        if (!entities.Any() || string.IsNullOrWhiteSpace(param.Name) && string.IsNullOrWhiteSpace(param.Status)) return;

        entities = entities.Where(r => r.Name!.Contains(param.Name!) || r.Status!.Contains(param.Status!));
    }
}