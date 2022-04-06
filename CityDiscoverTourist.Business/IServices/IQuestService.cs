using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface IQuestService
{
    public Task<Quest> Get(Guid id);
}