namespace CityDiscoverTourist.Business.IServices;

public interface IDashboardService
{
    public string?[] GetTopCustomer();
    public float GetTotalRevenue();
    public int TotalAccount();
    public int TotalQuest();
    public string[] GetTopQuests();
}