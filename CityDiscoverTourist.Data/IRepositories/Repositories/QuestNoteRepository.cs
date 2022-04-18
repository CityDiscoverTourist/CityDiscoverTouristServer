using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class QuestNoteRepository : GenericRepository<QuestNote, int>, IQuestNoteRepository
{
    public QuestNoteRepository(ApplicationDbContext context) : base(context)
    {
    }
}