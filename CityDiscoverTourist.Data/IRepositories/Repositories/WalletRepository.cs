using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class WalletRepository : GenericRepository<Wallet, int>, IWalletRepository
{
    public WalletRepository(ApplicationDbContext context) : base(context)
    {
    }
}