using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface INoteService
{
    public PageList<NoteResponseModel> GetAll(NoteParams @params);
    public Task<NoteResponseModel> Get(int id);
    public Task<NoteResponseModel> CreateAsync(NoteRequestModel request);
    public Task<NoteResponseModel> UpdateAsync(NoteRequestModel request);
    public Task<NoteResponseModel> DeleteAsync(int id);
}