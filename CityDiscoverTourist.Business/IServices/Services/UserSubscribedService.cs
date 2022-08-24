using CityDiscoverTourist.Business.Helper.EmailHelper;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class UserSubscribedService : IUserSubscribedService
{
    private readonly IUserSubscribedRepository _userSubscribedRepository;
    private readonly IEmailSender _emailSender;

    public UserSubscribedService(IUserSubscribedRepository userSubscribedRepository, IEmailSender emailSender)
    {
        _userSubscribedRepository = userSubscribedRepository;
        _emailSender = emailSender;
    }

    public Task<UserSubscribed> CreateAsync(UserSubscribed request)
    {
        return _userSubscribedRepository.Add(request);
    }

    public Task SendMailToSubscriber(string questName)
    {
        var subscriber = _userSubscribedRepository.GetAll();
        foreach (var item in subscriber)
        {
            var message = "Hello " + item.Name + " " + " New Quest " + questName + " have been created";
            _emailSender.SendMailConfirmAsync(item.Email!, "You have been subscribed to our newsletter", message);
        }
        return Task.CompletedTask;
    }
}