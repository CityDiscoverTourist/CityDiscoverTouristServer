using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.HubConfig.IHub;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.SignalR;

namespace CityDiscoverTourist.Business.HubConfig;

public class CustomerTaskHub : Hub<ICustomerTaskHub>
{
    public async Task AddCustomerTask(CustomerTaskResponseModel customerTask)
    {
        await Clients.All.AddCustomerTask(customerTask);
    }

    public async Task CustomerStartNextQuestItem(CustomerTaskResponseModel customerTask)
    {
        await Clients.All.CustomerStartNextQuestItem(customerTask);
    }

    public async Task CustomerAnswer(CustomerAnswerResponseModel customerAnswer)
    {
        await Clients.All.CustomerAnswer(customerAnswer);
    }

    public async Task UpdateCustomerTask(CustomerTask customerTask)
    {
        await Clients.All.UpdateCustomerTask(customerTask);
    }
}