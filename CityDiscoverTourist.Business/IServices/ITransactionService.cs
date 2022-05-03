using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface ITransactionService
{
    public PageList<Transaction> GetAll(TransactionParams @params);
    public Task<Transaction> Get(int id);
    public Task<Transaction> CreateAsync(Transaction request);
    public Task<Transaction> UpdateAsync(Transaction request);
    public Task<Transaction> DeleteAsync(int id);
}