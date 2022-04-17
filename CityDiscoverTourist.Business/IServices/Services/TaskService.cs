using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using Task = CityDiscoverTourist.Data.Models.Task;

namespace CityDiscoverTourist.Business.IServices.Services;

public class TaskService: ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<Task> _sortHelper;
    private readonly IDataShaper<Task> _dataShaper;

    public TaskService(ITaskRepository taskRepository, IMapper mapper, ISortHelper<Task> sortHelper, IDataShaper<Task> dataShaper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
        _dataShaper = dataShaper;
    }

    public PageList<Entity> GetAll(TaskParams @params)
    {
        var listAll = _taskRepository.GetAll();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var shapedData = _dataShaper.ShapeData(sortedQuests, @params.Fields);

        return PageList<Entity>.ToPageList(shapedData, @params.PageNume, @params.PageSize);
    }

    public async Task<TaskResponseModel> Get(int id, string? fields)
    {
        var entity = await _taskRepository.Get(id);

        //var shaped = _dataShaper.ShapeData(entity, fields);

        return _mapper.Map<TaskResponseModel>(entity);
    }

    public async Task<TaskResponseModel> CreateAsync(TaskRequestModel request)
    {
        var entity = _mapper.Map<Task>(request);
        entity = await _taskRepository.Add(entity);
        return _mapper.Map<TaskResponseModel>(entity);
    }

    public async Task<TaskResponseModel> UpdateAsync(TaskRequestModel request)
    {
        var entity = _mapper.Map<Task>(request);
        entity = await _taskRepository.Update(entity);
        return _mapper.Map<TaskResponseModel>(entity);
    }

    public async Task<TaskResponseModel> DeleteAsync(int id)
    {
        var entity = await _taskRepository.Delete(id);
        return _mapper.Map<TaskResponseModel>(entity);
    }

    private static void Search(ref IQueryable<Task> entities, TaskParams param)
    {
        if (!entities.Any() || string.IsNullOrWhiteSpace(param.Name) && string.IsNullOrWhiteSpace(param.Description)) return;

        entities = entities.Where(x => x.Description!.Contains(param.Description!));
    }
}