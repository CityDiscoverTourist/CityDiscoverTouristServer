using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class NoteService: BaseService, INoteService
{
    private readonly INoteRepository _noteRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<Note> _sortHelper;

    public NoteService(INoteRepository noteRepository, IMapper mapper, ISortHelper<Note> sortHelper)
    {
        _noteRepository = noteRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }

    public PageList<NoteResponseModel> GetAll(NoteParams @params)
    {
        var listAll = _noteRepository.GetAll();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        //var shapedData = _dataShaper.ShapeData(sortedQuests, param.Fields);
        var mappedData = _mapper.Map<IEnumerable<NoteResponseModel>>(sortedQuests);
        return PageList<NoteResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }

    public async Task<NoteResponseModel> Get(int id)
    {
        var entity = await _noteRepository.Get(id);
        CheckDataNotNull("Note", entity);
        return _mapper.Map<NoteResponseModel>(entity);
    }

    public async Task<NoteResponseModel> CreateAsync(NoteRequestModel request)
    {
        var entity = _mapper.Map<Note>(request);
        entity = await _noteRepository.Add(entity);
        return _mapper.Map<NoteResponseModel>(entity);
    }

    public async Task<NoteResponseModel> UpdateAsync(NoteRequestModel request)
    {
        var entity = _mapper.Map<Note>(request);
        entity = await _noteRepository.Update(entity);
        return _mapper.Map<NoteResponseModel>(entity);
    }

    public async Task<NoteResponseModel> DeleteAsync(int id)
    {
        var entity = await _noteRepository.Delete(id);
        return _mapper.Map<NoteResponseModel>(entity);
    }

    private static void Search(ref IQueryable<Note> entities, NoteParams param)
    {
        if (!entities.Any()) return;

        if(param.Content != null)
        {
            entities = entities.Where(r => r.Content!.Equals(param.Content));
        }
        if (param.CustomerTaskId != 0)
        {
            entities = entities.Where(r => r.CustomerTaskId == param.CustomerTaskId);
        }
    }
}