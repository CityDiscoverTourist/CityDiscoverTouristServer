using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class WalletService: IWalletService
{
    private readonly IWalletRepository _walletRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<Wallet> _sortHelper;

    public WalletService(IWalletRepository walletRepository, IMapper mapper, ISortHelper<Wallet> sortHelper)
    {
        _walletRepository = walletRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }

    public PageList<Wallet> GetAll(WalletParams @params)
    {
        var listAll = _walletRepository.GetAll();

        //Search(ref listAll, param);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        //var shapedData = _dataShaper.ShapeData(sortedQuests, param.Fields);
        var mappedData = _mapper.Map<IEnumerable<Wallet>>(sortedQuests);
        return PageList<Wallet>.ToPageList(mappedData, @params.PageNume, @params.PageSize);
    }

    public async Task<Wallet> Get(int id)
    {
        var entity = await _walletRepository.Get(id);

        return _mapper.Map<Wallet>(entity);
    }

    public async Task<Wallet> CreateAsync(WalletRequestModel request)
    {
        var entity = _mapper.Map<Wallet>(request);
        entity = await _walletRepository.Add(entity);
        return _mapper.Map<Wallet>(entity);
    }

    public async Task<Wallet> UpdateAsync(WalletRequestModel request)
    {
        var entity = _mapper.Map<Wallet>(request);
        entity = await _walletRepository.Update(entity);
        return _mapper.Map<Wallet>(entity);
    }

    public async Task<Wallet> DeleteAsync(int id)
    {
        var entity = await _walletRepository.Delete(id);
        return _mapper.Map<Wallet>(entity);
    }

    /*private static void Search(ref IQueryable<Wallet> entities, QuestParams param)
    {
        if (!entities.Any()) return;

        if(param.Name != null)
        {
            entities = entities.Where(r => r.Title!.Contains(param.Name));
        }
        if (param.Description != null)
        {
            entities = entities.Where(r => r.Description!.Contains(param.Description));
        }
        if (param.Status != null)
        {
            entities = entities.Where(r => r.Status!.Contains(param.Status));
        }
    }*/
}