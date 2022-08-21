using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CityDiscoverTourist.Business.IServices.Services;

public class RewardService : BaseService, IRewardService
{
    private readonly IMapper _mapper;
    private readonly IRewardRepository  _rewardRepository;
    private readonly ISortHelper<Reward> _sortHelper;
    private static UserManager<ApplicationUser>? _userManager;
    private readonly INotificationService _notificationService;

    public RewardService(IRewardRepository rewardRepository, IMapper mapper, ISortHelper<Reward> sortHelper, UserManager<ApplicationUser>? userManager, INotificationService notificationService)
    {
        _rewardRepository = rewardRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
        _userManager = userManager;
        _notificationService = notificationService;
    }

    public PageList<RewardResponseModel> GetAll(RewardParams @params)
    {
        var listAll = _rewardRepository.GetAll().OrderByDescending(x => x.ReceivedDate).AsNoTracking();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var mappedData = _mapper.Map<IEnumerable<RewardResponseModel>>(sortedQuests);

        foreach (var item in mappedData)
        {
            item.CustomerEmail = _userManager!.FindByIdAsync(item.CustomerId).Result.Email;
        }

        return PageList<RewardResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }

    public PageList<RewardResponseModel> GetByCustomerId(RewardParams @params, string customerId)
    {
        var rewards = _rewardRepository.GetByCondition(x => x.CustomerId == customerId && x.Status == CommonStatus.Active.ToString())
            .OrderByDescending(x => x.ReceivedDate).AsNoTracking();

        var sortedQuests = _sortHelper.ApplySort(rewards, @params.OrderBy);
        var mappedData = _mapper.Map<IEnumerable<RewardResponseModel>>(sortedQuests);

        return PageList<RewardResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }

    public async Task<RewardResponseModel> Get(int id)
    {
        var entity = await _rewardRepository.Get(id);
        CheckDataNotNull("Reward", entity);
        return _mapper.Map<RewardResponseModel>(entity);
    }

    public async Task<RewardResponseModel> CreateAsync(RewardRequestModel request)
    {
        var entity = _mapper.Map<Reward>(request);
        entity.ExpiredDate = CurrentDateTime().AddDays(7);
        entity = await _rewardRepository.Add(entity);
        return _mapper.Map<RewardResponseModel>(entity);
    }

    public async Task<RewardResponseModel> InvalidReward()
    {
        var entity = _rewardRepository.GetAll();

        foreach (var item in entity)
        {
            if (item.Status == CommonStatus.Inactive.ToString()) continue;
            if (!(item.ExpiredDate!.Value.Date < CurrentDateTime().Date)) continue;

            item.Status = CommonStatus.Inactive.ToString();
            await _rewardRepository.UpdateFields(item, x => x.Status!);
        }

        return null!;
    }

    public async Task<RewardResponseModel> DeleteAsync(int id)
    {
        var entity = await _rewardRepository.Delete(id);
        return _mapper.Map<RewardResponseModel>(entity);
    }

    public Task PushNotification()
    {
        var users = _userManager!.Users;
        foreach (var user in users)
        {
            var reward = _rewardRepository.GetByCondition(x => x.Status == CommonStatus.Active.ToString())
                .Where(x => x.CustomerId == user.Id);

            foreach (var item in reward)
            {
                // send notification to user when reward has 1 day left to expire
                if (item.ExpiredDate!.Value.Date == CurrentDateTime().Date.AddDays(1))
                {
                    _notificationService.SendNotification(new NotificationRequestModel
                    {
                        DeviceId = user.DeviceId,
                        IsAndroidDevice = true,
                        Title = "Your reward has 1 day left to expire",
                        Body = "Your reward has 1 day left to expire, come back app and use it"
                    });
                }
            }
        }
        return Task.CompletedTask;
    }

    private static void Search(ref IQueryable<Reward> entities, RewardParams param)
    {
        if (!entities.Any()) return;

        if (param.Name != null) entities = entities.Where(x => x.Name!.Equals(param.Name));
        if (param.ExpiredDate != null) entities = entities.Where(x => x.ExpiredDate >= param.ExpiredDate);
        if (param.ReceivedDate != null) entities = entities.Where(x => x.ReceivedDate >= param.ReceivedDate);
        if (param.CustomerEmail != null)
        {
            var customerId = _userManager!.FindByEmailAsync(param.CustomerEmail).Result != null
                ? _userManager.FindByEmailAsync(param.CustomerEmail).Result.Id
                : null;
            entities = entities.Where(x => x.CustomerId == customerId);
        }
        if(param.Status != null) entities = entities.Where(x => x.Status == param.Status);
    }
}