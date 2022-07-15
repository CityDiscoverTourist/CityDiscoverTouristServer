using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.HubConfig.IHub;

public interface ICustomerTaskHub
{
    Task AddCustomerTask(CustomerTaskResponseModel customerTask);
    Task CustomerStartNextQuestItem(CustomerTaskResponseModel customerTask);
    Task UpdateCustomerTask(CustomerTask customerTask);
    Task CustomerAnswer(CustomerAnswerResponseModel customerAnswer);
}