using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface ICustomerAnswerService
{
    public PageList<CustomerAnswerResponseModel> GetAll(CustomerAnswerParams @params);
    public Task<CustomerAnswerResponseModel> Get(int id);
    public Task<List<CustomerAnswerResponseModel>> GetByCustomerTaskId(int customerTaskId);
    public Task<CustomerAnswerResponseModel> CreateAsync(CustomerAnswerRequestModel request);
    public Task<CustomerAnswerResponseModel> UpdateAsync(CustomerAnswerRequestModel request);
    public Task<CustomerAnswerResponseModel> DeleteAsync(int id);
}