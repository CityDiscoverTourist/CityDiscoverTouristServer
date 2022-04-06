using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class QuestSerivce: IQuestService
{
    private readonly IQuestRepository _questRepository;

    public QuestSerivce(IQuestRepository questRepository)
    {
        _questRepository = questRepository;
    }

    public Task<Quest> Get(Guid id)
    {
        return _questRepository.Get(id);
    }
}