using System.Globalization;
using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Exceptions;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace CityDiscoverTourist.Business.IServices.Services;

public class CustomerQuestService : BaseService, ICustomerQuestService
{
    private const int BaseMultiplier = 300;
    private static  UserManager<ApplicationUser>? _userManager;
    private readonly ICustomerQuestRepository _customerQuestRepository;
    private readonly ICustomerTaskService _customerTaskService;
    private readonly IMapper _mapper;
    private readonly ISortHelper<CustomerQuest> _sortHelper;
    private readonly IQuestItemRepository _taskRepository;
    private readonly IPaymentService _paymentService;

    public CustomerQuestService(ICustomerQuestRepository customerQuestRepository, IMapper mapper,
        IQuestItemRepository taskRepository, ISortHelper<CustomerQuest> sortHelper,
        ICustomerTaskService customerTaskService, UserManager<ApplicationUser>? userManager, IPaymentService paymentService)
    {
        _customerQuestRepository = customerQuestRepository;
        _mapper = mapper;
        _taskRepository = taskRepository;
        _sortHelper = sortHelper;
        _customerTaskService = customerTaskService;
        _userManager = userManager;
        _paymentService = paymentService;
    }

    public PageList<CustomerQuestResponseModel> GetAll(CustomerQuestParams @params)
    {
        var listAll = _customerQuestRepository.GetAll();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);

        var mappedData = _mapper.Map<IEnumerable<CustomerQuestResponseModel>>(sortedQuests);
        return PageList<CustomerQuestResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }

    public async Task<CustomerQuestResponseModel> Get(int id)
    {
        var entity = await _customerQuestRepository.Get(id);
        CheckDataNotNull("CustomerQuest", entity);
        return _mapper.Map<CustomerQuestResponseModel>(entity);
    }

    public  Task<List<CustomerQuestResponseModel>> GetByCustomerId(string id)
    {
        var entity = _customerQuestRepository.GetByCondition(x => x.CustomerId == id).ToList();
        CheckDataNotNull("CustomerQuest", entity);
        var mappedData = _mapper.Map<IEnumerable<CustomerQuestResponseModel>>(entity);
        return Task.FromResult(mappedData.ToList());
    }

    public async Task<CustomerQuestResponseModel> CreateAsync(CustomerQuestRequestModel request)
    {
        var entity = _mapper.Map<CustomerQuest>(request);

        var payment = _paymentService.Get(entity.PaymentId).Result;
        if(!payment.IsValid) throw new AppException("Payment is not valid");

        // get quantity of the order
        var ticketQuantity = _paymentService.GetQuantityOfPayment(entity.PaymentId);

        // count number of order show in customer quest
        var numOfQuantityInCustomerQuest = _customerQuestRepository
            .GetByCondition(x => x.PaymentId == entity.PaymentId).Count();

        if (numOfQuantityInCustomerQuest >= ticketQuantity) throw new AppException("Ticket quantity is not enough");

        entity.IsFinished = false;
        entity.Status = PaymentStatus.Success.ToString();
        entity.BeginPoint = CalculateBeginPoint(request.QuestId);
        entity.QuestId = payment.QuestId;

        entity = await _customerQuestRepository.Add(entity);
        return _mapper.Map<CustomerQuestResponseModel>(entity);
    }

    public async Task<CustomerQuestResponseModel> DeleteAsync(int id)
    {
        var entity = await _customerQuestRepository.Delete(id);
        return _mapper.Map<CustomerQuestResponseModel>(entity);
    }

    public async Task<CustomerQuestResponseModel> UpdateEndPointAndStatusWhenFinishQuestAsync(int customerQuestId)
    {
        var isLastItem = await _customerTaskService.IsLastQuestItem(customerQuestId);

        if (!isLastItem) throw new AppException("Quest is not finished");

        var lastPoint = _customerTaskService.GetLastPoint(customerQuestId);
        var entity = await  _customerQuestRepository.Get(customerQuestId);

        entity.EndPoint = lastPoint.ToString(CultureInfo.InvariantCulture);
        entity.Status = CommonStatus.Done.ToString();
        entity.IsFinished = true;
        entity = await _customerQuestRepository.UpdateFields(entity, x => x.EndPoint!, x => x.Status!,
            x => x.IsFinished);

        return _mapper.Map<CustomerQuestResponseModel>(entity);
    }

    public Task<PageList<CommentResponseModel>> ShowComments(int questId, CustomerQuestParams param)
    {
        var comments = _customerQuestRepository.GetAll().Where(x => x.QuestId == questId && x.IsFinished == true)
            .OrderByDescending(x => x.CreatedDate);

        var mappedData = _mapper.Map<IEnumerable<CommentResponseModel>>(comments);

        var commentResponseModels = mappedData.ToList();
        foreach (var comment in commentResponseModels)
        {
            var customerName = _userManager!.FindByIdAsync(comment.CustomerId).Result.UserName;
            var imagePath = _userManager.FindByIdAsync(comment.CustomerId).Result.ImagePath;

            comment.ImagePath = imagePath;
            comment.Name = customerName;
        }

        return Task.FromResult(PageList<CommentResponseModel>.ToPageList(commentResponseModels, param.PageNumber, param.PageSize));
    }

    public Task<List<CommentResponseModel>> UpdateComment(int questId, string customerId, CommentRequestModel request)
    {
        var comments = GetComment(questId, customerId);

        var mappedData = _mapper.Map<IEnumerable<CustomerQuest>>(comments).FirstOrDefault();

        mappedData!.FeedBack = request.FeedBack;
        mappedData.Rating = request.Rating;

        _customerQuestRepository.UpdateFields(mappedData, x => x.FeedBack!, x => x.Rating);

        return null!;
    }

    public IQueryable<CustomerQuest> GetMyComment( int questId, string customerId)
    {
        return GetComment(questId, customerId);
    }

    private static void Search(ref IQueryable<CustomerQuest> entities, CustomerQuestParams param)
    {
        if (!entities.Any()) return;

        if (param.QuestId != 0)
            entities = entities.Where(x => x.QuestId == param.QuestId);
        if (param.CustomerEmail != null)
        {
            var customerId = _userManager!.FindByEmailAsync(param.CustomerEmail).Result.Id;
            entities = entities.Where(x => x.CustomerId == customerId);
        }
        if(param.IsFinished != null)
            entities = entities.Where(x => x.IsFinished == param.IsFinished);

    }

    private int CountQuestItemInQuest(int questId)
    {
        var questItems = _taskRepository.GetByCondition(x => x.QuestId == questId);
        return questItems.Count();
    }

    private string CalculateBeginPoint(int questId)
    {
        return (CountQuestItemInQuest(questId) * BaseMultiplier).ToString();
    }

    private IQueryable<CustomerQuest> GetComment(int questId, string customerId)
    {
        var comments = _customerQuestRepository.GetAll()
            .Where(x => x.QuestId == questId && x.IsFinished == true && x.CustomerId == customerId);
        return comments;
    }
}