using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class QuestRewardService : IQuestRewardService
{
    private readonly IMapper _mapper;
    private readonly IQuestRewardRepository _questRewardRepository;

    public QuestRewardService(IMapper mapper, IQuestRewardRepository questRewardRepository)
    {
        _mapper = mapper;
        _questRewardRepository = questRewardRepository;
    }

    public async Task<QuestRewardResponseModel> CreateAsync(QuestRewardRequestModel request)
    {
        var entity = _mapper.Map<QuestReward>(request);
        var result = await _questRewardRepository.Add(entity);
        return _mapper.Map<QuestRewardResponseModel>(result);
    }
}