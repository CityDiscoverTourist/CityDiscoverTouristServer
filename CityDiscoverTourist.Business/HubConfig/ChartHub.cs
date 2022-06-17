using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.SignalR;

namespace CityDiscoverTourist.Business.HubConfig;

public class ChartHub: Hub
{
    public async Task BroadcastChartData(List<ChartModel> data) =>
        await Clients.All.SendAsync("broadcastchartdata", data);
}