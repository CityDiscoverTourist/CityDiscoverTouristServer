using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface IWalletService
{
    public PageList<WalletResponseModel> GetAll(WalletParams @params);
    public Task<WalletResponseModel> Get(int id);
    public Task<WalletResponseModel> CreateAsync(WalletRequestModel request);
    public Task<WalletResponseModel> UpdateAsync(WalletRequestModel request);
    public Task<WalletResponseModel> DeleteAsync(int id);
}