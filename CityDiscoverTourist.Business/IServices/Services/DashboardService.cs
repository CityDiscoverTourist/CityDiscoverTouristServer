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

    public string?[] GetTopCustomer()
    {
        /*var list = new List<string>();
        var payments = _paymentRepository.GetAll().Where(x => x.Status == PaymentStatus.Success.ToString());

        foreach (var payment in payments)
        {
            var user = _userManager.FindByIdAsync(payment.CustomerId).Result;
            if (user.Id == payment.CustomerId)
            {
                var customer = "";
            }

        }*/
        // get top 3 customers by total amount of payments
        var topCustomers = _paymentRepository.GetAll().Where(x => x.Status == PaymentStatus.Success.ToString())
            .GroupBy(x => x.CustomerId).Select(x => new { CustomerId = x.Key, TotalAmount = x.Sum(y => y.TotalAmount) })
            .OrderByDescending(x => x.TotalAmount).Take(3);
        var list = new List<string>();
        foreach (var customer in topCustomers)
        {
            var user = _userManager.FindByIdAsync(customer.CustomerId).Result;
            if (user.Id == customer.CustomerId)
            {
                list.Add(user.UserName);
                list.Add(customer.TotalAmount.ToString());
            }
        }

        return list.ToArray();
    }

    public float GetTotalRevenue()
    {
        var payments = _paymentRepository.GetAll().Where(x => x.Status == PaymentStatus.Success.ToString()).ToList();

        return payments.Sum(payment => payment.TotalAmount);
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
            .Select(x => new { QuestId = x.Key, TotalPlay = x.Count() }).OrderByDescending(x => x.TotalPlay).Take(3);
        var list = new List<string>();
        foreach (var quest in topQuests)
        {
            var questName = ConvertLanguage(Language.vi, _questRepository.Get(quest.QuestId).Result.Title!);
            list.Add(questName);
        }

        return list.ToArray();
    }
}