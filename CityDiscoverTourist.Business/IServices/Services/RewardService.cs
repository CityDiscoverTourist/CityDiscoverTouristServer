using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace CityDiscoverTourist.Business.IServices.Services;

public class RewardService : BaseService, IRewardService
{
    private readonly IMapper _mapper;
    private readonly IRewardRepository  _rewardRepository;
    private readonly ISortHelper<Reward> _sortHelper;
    private static UserManager<ApplicationUser>? _userManager;


    public RewardService(IRewardRepository rewardRepository, IMapper mapper, ISortHelper<Reward> sortHelper, UserManager<ApplicationUser>? userManager)
    {
        _rewardRepository = rewardRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
        _userManager = userManager;
    }

    public PageList<RewardResponseModel> GetAll(RewardParams @params)
    {
        var listAll = _rewardRepository.GetAll();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var mappedData = _mapper.Map<IEnumerable<RewardResponseModel>>(sortedQuests);

        return PageList<RewardResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }

    public PageList<RewardResponseModel> GetByCustomerId(RewardParams @params, string customerId)
    {
        var rewards = _rewardRepository.GetByCondition(x => x.CustomerId == customerId);

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
        entity.ExpiredDate = DateTime.UtcNow.AddDays(7);
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
    }
}