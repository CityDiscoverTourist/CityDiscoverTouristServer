using static CityDiscoverTourist.Business.IServices.Services.DashboardService;

namespace CityDiscoverTourist.Business.IServices;

public interface IDashboardService
{
    public Player[] GetTopCustomer();
    public float GetTotalRevenue();
    public int TotalAccount();
    public int TotalQuest();
    public string[] GetTopQuests();
}