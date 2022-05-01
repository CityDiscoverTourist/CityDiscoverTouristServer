using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class NoteService: INoteService
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

    public PageList<Note> GetAll(NoteParams @params)
    {
        var listAll = _noteRepository.GetAll();

        //Search(ref listAll, param);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        //var shapedData = _dataShaper.ShapeData(sortedQuests, param.Fields);
        var mappedData = _mapper.Map<IEnumerable<Note>>(sortedQuests);
        return PageList<Note>.ToPageList(mappedData, @params.PageNume, @params.PageSize);
    }

    public async Task<Note> Get(int id)
    {
        var entity = await _noteRepository.Get(id);

        return _mapper.Map<Note>(entity);
    }

    public async Task<Note> CreateAsync(Note request)
    {
        var entity = _mapper.Map<Note>(request);
        entity = await _noteRepository.Add(entity);
        return _mapper.Map<Note>(entity);
    }

    public async Task<Note> UpdateAsync(Note request)
    {
        var entity = _mapper.Map<Note>(request);
        entity = await _noteRepository.Update(entity);
        return _mapper.Map<Note>(entity);
    }

    public async Task<Note> DeleteAsync(int id)
    {
        var entity = await _noteRepository.Delete(id);
        return _mapper.Map<Note>(entity);
    }
}