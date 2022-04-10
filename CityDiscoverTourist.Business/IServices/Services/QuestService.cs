using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class QuestService: IQuestService
{
    private readonly IQuestRepository _questRepository;

    public QuestService(IQuestRepository questRepository)
    {
        _questRepository = questRepository;
    }

    public Task<Quest> Get(Guid id)
    {
        return _questRepository.Get(id);
    }
}