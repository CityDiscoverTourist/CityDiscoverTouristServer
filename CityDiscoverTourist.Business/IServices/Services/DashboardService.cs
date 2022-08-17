using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace CityDiscoverTourist.Business.IServices.Services;

public class DashboardService : BaseService, IDashboardService
{
    private readonly ICustomerQuestRepository _customerQuestRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IQuestRepository _questRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public DashboardService(IPaymentRepository paymentRepository, UserManager<ApplicationUser> userManager,
        IQuestRepository questRepository, ICustomerQuestRepository customerQuestRepository)
    {
        _paymentRepository = paymentRepository;
        _userManager = userManager;
        _questRepository = questRepository;
        _customerQuestRepository = customerQuestRepository;
    }

   public class Player
    {
        public string email { get; set; }
        public string point { get; set; }
    }

    public class QuestDashboard
    {
        public string name { get; set; }
        public string count { get; set; }
    }

    public Player[] GetTopCustomer()
    {
        // get top 3 customers by total amount of payments
        var topCustomers = _paymentRepository.GetAll().Where(x => x.Status == PaymentStatus.Success.ToString())
            .GroupBy(x => x.CustomerId).Select(x => new { CustomerId = x.Key, TotalAmount = x.Sum(y => y.TotalAmount) })
            .OrderByDescending(x => x.TotalAmount).Take(10);
        var list = new List<Player>();
        foreach (var customer in topCustomers)
        {
            var user = _userManager.FindByIdAsync(customer.CustomerId).Result;
            if (user.Id == customer.CustomerId)
            {
                Player player = new Player
                {
                    email = user.UserName!,
                    point = customer.TotalAmount.ToString()!
                };
                list.Add(player);
            }
        }

        return list.ToArray();
    }

    public float GetTotalRevenue()
    {
        var payments = _paymentRepository.GetAll().Where(x => x.Status == PaymentStatus.Success.ToString()).ToList();

        return payments.Sum(payment => payment.TotalAmount);
    }

    public float GetRevenueByMonthOfYear(int month, int year)
    {
        var payments = _paymentRepository.GetAll().Where(x => x.Status == PaymentStatus.Success.ToString()).ToList();
        var paymentsByMonth = payments.Where(payment => payment.CreatedDate.Month == month && payment.CreatedDate.Year == year).ToList();
        return paymentsByMonth.Sum(payment => payment.TotalAmount);
    }

    public float[] GetRevenueByAllMonth(int year)
    {
        var payments = _paymentRepository.GetAll().Where(x => x.Status == PaymentStatus.Success.ToString()).ToList();
        var paymentsByMonth = payments.Where(payment => payment.CreatedDate.Year == year).ToList();
        var revenues = new float[12];
        for (var i = 0; i < 12; i++)
        {
            revenues[i] = paymentsByMonth.Where(payment => payment.CreatedDate.Month == i + 1).Sum(payment => payment.TotalAmount);
        }

        return revenues;
    }

    public float GetRevenueByMonth(int month)
    {
        var payments = _paymentRepository.GetAll().Where(x => x.Status == PaymentStatus.Success.ToString()).ToList();
        var paymentsByMonth = payments.Where(payment => payment.CreatedDate.Month == month).ToList();
        return paymentsByMonth.Sum(payment => payment.TotalAmount);
    }

    public int TotalAccount()
    {
        return _userManager.Users.Count();
    }

    public int TotalQuest()
    {
        return _questRepository.GetAll().Count();
    }

    public string[] GetTopQuests()
    {
        // get top quest play most
        var topQuests = _customerQuestRepository.GetAll().GroupBy(x => x.QuestId)
            .Select(x => new { QuestId = x.Key, TotalPlay = x.Count() }).OrderByDescending(x => x.TotalPlay).Take(10);
        var list = new List<string>();
        foreach (var quest in topQuests)
        {
            var questName = ConvertLanguage(Language.vi, _questRepository.Get(quest.QuestId).Result.Title!);
            list.Add(questName);
        }

        return list.ToArray();
    }

    public QuestDashboard[] GetTopQuestByMonth(int month, int year)
    {
        var topQuests = _customerQuestRepository.GetAll().Where(x => x.CreatedDate!.Value.Month == month && x.CreatedDate.Value.Year == year)
            .GroupBy(x => x.QuestId).Select(x => new { QuestId = x.Key, TotalPlay = x.Count() }).OrderByDescending(x => x.TotalPlay).Take(10);
        var list = new List<QuestDashboard>();
        foreach (var quest in topQuests)
        {
            var questName = ConvertLanguage(Language.vi, _questRepository.Get(quest.QuestId).Result.Title!);
            QuestDashboard quest1 = new QuestDashboard
            {
                name = questName,
                count = quest.TotalPlay.ToString()
            };
            list.Add(quest1);
        }

        return list.ToArray();
    }

    public QuestDashboard[] GetTopQuestByMonthInYear(int year = 2022)
    {
        var topQuests = _customerQuestRepository.GetAll().Where(x => x.CreatedDate!.Value.Year == year)
            .GroupBy(x => x.QuestId).Select(x => new { QuestId = x.Key, TotalPlay = x.Count() }).OrderByDescending(x => x.TotalPlay).Take(10);
        var list = new List<QuestDashboard>();
        foreach (var quest in topQuests)
        {
            var questName = ConvertLanguage(Language.vi, _questRepository.Get(quest.QuestId).Result.Title!);
            QuestDashboard quest1 = new QuestDashboard
            {
                name = questName,
                count = quest.TotalPlay.ToString()
            };
            list.Add(quest1);
        }

        return list.ToArray();
    }
}