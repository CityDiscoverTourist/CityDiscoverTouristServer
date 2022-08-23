using static CityDiscoverTourist.Business.IServices.Services.DashboardService;

namespace CityDiscoverTourist.Business.IServices;

public interface IDashboardService
{
    public Player[] GetTopCustomer();
    public float GetTotalRevenue();
    public float GetRevenueByMonthOfYear(int month, int year);
    public float[] GetRevenueByAllMonth(int year);
    public int TotalAccount();
    public int TotalQuest();
    public QuestDashboard[] GetTopQuests(int year);
    public QuestDashboard[] GetTopQuestByMonth(int month, int year = 2022);
    public Task<QuestDashboard[]> GetTopQuestByMonthInYear(int year = 2022);

}