using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class TransactionService: BaseService, ITransactionService
{
    private readonly ITransactionRepository _transRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<Transaction> _sortHelper;

    public TransactionService(ITransactionRepository transRepository, IMapper mapper, ISortHelper<Transaction> sortHelper)
    {
        _transRepository = transRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }

    public PageList<Transaction> GetAll(TransactionParams @params)
    {
        var listAll = _transRepository.GetAll();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        //var shapedData = _dataShaper.ShapeData(sortedQuests, param.Fields);
        var mappedData = _mapper.Map<IEnumerable<Transaction>>(sortedQuests);
        return PageList<Transaction>.ToPageList(mappedData, @params.PageNume, @params.PageSize);
    }

    public async Task<Transaction> Get(int id)
    {
        var entity = await _transRepository.Get(id);
        CheckDataNotNull("Transaction", entity);
        return _mapper.Map<Transaction>(entity);
    }

    public async Task<Transaction> CreateAsync(TransactionRequestModel request)
    {
        var entity = _mapper.Map<Transaction>(request);
        entity = await _transRepository.Add(entity);
        return _mapper.Map<Transaction>(entity);
    }

    public async Task<Transaction> UpdateAsync(TransactionRequestModel request)
    {
        var entity = _mapper.Map<Transaction>(request);
        entity = await _transRepository.Update(entity);
        return _mapper.Map<Transaction>(entity);
    }

    public async Task<Transaction> DeleteAsync(int id)
    {
        var entity = await _transRepository.Delete(id);
        return _mapper.Map<Transaction>(entity);
    }

    private static void Search(ref IQueryable<Transaction> entities, TransactionParams param)
    {
        if (!entities.Any()) return;

        if(param.Type != null)
        {
            entities = entities.Where(r => r.TypeTransaction!.Equals(param.Type));
        }
        if (param.Total != 0)
        {
            entities = entities.Where(r => r.Total == param.Total);
        }
        if (param.WalletId != 0)
        {
            entities = entities.Where(r => r.WalletId == param.WalletId);
        }
    }
}