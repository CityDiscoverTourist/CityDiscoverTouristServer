using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface IWalletService
{
    public PageList<Wallet> GetAll(WalletParams @params);
    public Task<Wallet> Get(int id);
    public Task<Wallet> CreateAsync(WalletRequestModel request);
    public Task<Wallet> UpdateAsync(WalletRequestModel request);
    public Task<Wallet> DeleteAsync(int id);
}