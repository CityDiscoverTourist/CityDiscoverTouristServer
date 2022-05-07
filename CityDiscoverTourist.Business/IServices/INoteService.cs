using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface INoteService
{
    public PageList<Note> GetAll(NoteParams @params);
    public Task<Note> Get(int id);
    public Task<Note> CreateAsync(NoteRequestModel request);
    public Task<Note> UpdateAsync(NoteRequestModel request);
    public Task<Note> DeleteAsync(int id);
}