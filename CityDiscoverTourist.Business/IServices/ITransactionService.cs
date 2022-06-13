using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface ITransactionService
{
    public PageList<TransactionResponseModel> GetAll(TransactionParams @params);
    public Task<TransactionResponseModel> Get(int id);
    public Task<TransactionResponseModel> CreateAsync(TransactionRequestModel request);
    public Task<TransactionResponseModel> UpdateAsync(TransactionRequestModel request);
    public Task<TransactionResponseModel> DeleteAsync(int id);
}