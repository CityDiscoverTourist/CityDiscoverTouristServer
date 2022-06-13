using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class RewardService : BaseService, IRewardService
{
    private readonly IMapper _mapper;
    private readonly IRewardRepository  _rewardRepository;
    private readonly ISortHelper<Reward> _sortHelper;

    public RewardService(IRewardRepository rewardRepository, IMapper mapper, ISortHelper<Reward> sortHelper)
    {
        _rewardRepository = rewardRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }

    public PageList<RewardResponseModel> GetAll(RewardParams @params)
    {
        var listAll = _rewardRepository.GetAll();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
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
        entity = await _rewardRepository.Add(entity);
        return _mapper.Map<RewardResponseModel>(entity);
    }

    public async Task<RewardResponseModel> UpdateAsync(RewardRequestModel request)
    {
        var entity = _mapper.Map<Reward>(request);
        entity = await _rewardRepository.Update(entity);
        return _mapper.Map<RewardResponseModel>(entity);
    }

    public async Task<RewardResponseModel> DeleteAsync(int id)
    {
        var entity = await _rewardRepository.Delete(id);
        return _mapper.Map<RewardResponseModel>(entity);
    }

    private static void Search(ref IQueryable<Reward> entities, RewardParams param)
    {
        if (!entities.Any() || string.IsNullOrWhiteSpace(param.Name) && !param.ExpiredDate.HasValue) return;

        if (param.Name != null) entities = entities.Where(x => x.Name.Equals(param.Name));
        if (param.ExpiredDate != null) entities = entities.Where(x => x.ExpiredDate >= param.ExpiredDate);
        if (param.ReceivedDate != null) entities = entities.Where(x => x.ReceivedDate >= param.ReceivedDate);
    }
}