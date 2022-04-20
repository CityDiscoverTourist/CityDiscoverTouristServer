using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Task = CityDiscoverTourist.Data.Models.Task;

namespace CityDiscoverTourist.Business.IServices.Services;

public class NoteService: INoteService
{
    private readonly INoteRepository _noteRepository;
    private readonly IMapper _mapper;

    public NoteService(INoteRepository noteRepository, IMapper mapper)
    {
        _noteRepository = noteRepository;
        _mapper = mapper;
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