using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;

namespace CityDiscoverTourist.Business.IServices;

public interface IExperienceService
{
    public Task<ExperienceResponseModel> Get(int id);
    public Task<ExperienceResponseModel> CreateAsync(ExperienceRequestModel request);
    public Task<ExperienceResponseModel> UpdateAsync(ExperienceRequestModel request);
    public Task<ExperienceResponseModel> DeleteAsync(int id);
}