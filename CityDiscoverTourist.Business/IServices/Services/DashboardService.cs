using CityDiscoverTourist.Data.IRepositories;

namespace CityDiscoverTourist.Business.IServices.Services;

public class DashboardService : IDashboardService
{
    private readonly IPaymentRepository _paymentRepository;

    public DashboardService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public string?[] GetTopCustomer()
    {
        var payments = _paymentRepository.GetAll();
        var topCustomers = payments.GroupBy(p => p.CustomerId)
            .OrderByDescending(p => p.Count()).Take(3)
            .Select(p => p.Key).ToArray();
        return topCustomers;
    }
}