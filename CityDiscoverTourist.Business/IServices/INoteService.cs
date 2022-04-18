using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface INoteService
{
    public Task<Note> Get(int id);
    public Task<Note> CreateAsync(Note request);
    public Task<Note> UpdateAsync(Note request);
    public Task<Note> DeleteAsync(int id);
}