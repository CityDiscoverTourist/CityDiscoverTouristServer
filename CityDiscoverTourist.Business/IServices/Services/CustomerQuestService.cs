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
using Microsoft.EntityFrameworkCore;
using static System.Double;

namespace CityDiscoverTourist.Business.IServices.Services;

public class CustomerQuestService : BaseService, ICustomerQuestService
{
    private const int BaseMultiplier = 300;
    private static  UserManager<ApplicationUser>? _userManager;
    private readonly ICustomerQuestRepository _customerQuestRepository;
    private readonly ICustomerTaskService _customerTaskService;
    private readonly IMapper _mapper;
    private readonly IPaymentService _paymentService;
    private readonly IRewardRepository _rewardRepository;
    private readonly ISortHelper<CustomerQuest> _sortHelper;
    private readonly IQuestItemRepository _taskRepository;
    private readonly ICustomerTaskRepository _customerTaskRepository;

    public CustomerQuestService(ICustomerQuestRepository customerQuestRepository, IMapper mapper,
        IQuestItemRepository taskRepository, ISortHelper<CustomerQuest> sortHelper,
        ICustomerTaskService customerTaskService, UserManager<ApplicationUser>? userManager,
        IPaymentService paymentService, IRewardRepository rewardRepository, ICustomerTaskRepository customerTaskRepository)
    {
        _customerQuestRepository = customerQuestRepository;
        _mapper = mapper;
        _taskRepository = taskRepository;
        _sortHelper = sortHelper;
        _customerTaskService = customerTaskService;
        _userManager = userManager;
        _paymentService = paymentService;
        _rewardRepository = rewardRepository;
        _customerTaskRepository = customerTaskRepository;
    }

    public PageList<CustomerQuestResponseModel> GetAll(CustomerQuestParams @params)
    {
        var listAll = _customerQuestRepository.GetAll()
            .OrderByDescending(x => x.CreatedDate).AsNoTracking();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);

        var mappedData = _mapper.Map<IEnumerable<CustomerQuestResponseModel>>(sortedQuests);

        var customerQuestResponseModels = mappedData as CustomerQuestResponseModel[] ?? mappedData.ToArray();

        foreach (var quest in customerQuestResponseModels)
        {
            var customerName = _userManager?.FindByIdAsync(quest.CustomerId).Result.UserName;
            quest.CustomerName = customerName;
        }

        return PageList<CustomerQuestResponseModel>.ToPageList(customerQuestResponseModels, @params.PageNumber,
            @params.PageSize);
    }

    public async Task<CustomerQuestResponseModel> Get(int id)
    {
        var entity = await _customerQuestRepository.Get(id);
        CheckDataNotNull("CustomerQuest", entity);

        var mappedData = _mapper.Map<CustomerQuestResponseModel>(entity);

        mappedData.CustomerName = _userManager?.FindByIdAsync(mappedData.CustomerId).Result.UserName;

        return mappedData;
    }

    public async Task<CustomerQuestResponseModel> GiveFeedback(int id, CommentRequestModel comment)
    {
        var entity = await _customerQuestRepository.Get(id);

        if(!entity.IsFinished) throw new AppException("Quest is not finished yet");

        entity.Rating = comment.Rating;
        entity.FeedBack = comment.FeedBack;
        entity.IsFeedbackApproved = true;

        var result = await _customerQuestRepository.UpdateFields(entity, x => x.Rating,
            x => x.FeedBack!, x => x.IsFeedbackApproved);

        return _mapper.Map<CustomerQuestResponseModel>(result);
    }

    public async Task<CustomerQuestResponseModel> InvalidCustomerQuest()
    {
        var entity = _customerQuestRepository.GetAll();

        foreach (var item in entity)
        {
            if (item.IsFinished) continue;
            if (!(item.CreatedDate!.Value.Date < CurrentDateTime().Date)) continue;

            var lastCustomerTask = _customerTaskRepository
                .GetByCondition(x => x.CustomerQuestId == item.Id).OrderByDescending(x => x.CreatedDate)
                .LastOrDefaultAsync().Result;

            lastCustomerTask!.IsFinished = true;
            lastCustomerTask.Status = CommonStatus.Done.ToString();

            await _customerTaskRepository.UpdateFields(lastCustomerTask, x => x.IsFinished, x => x.Status);

            item.Rating = 5;
            item.IsFinished = true;
            item.Status = CommonStatus.Inactive.ToString();

            await _customerQuestRepository.UpdateFields(item, x => x.IsFinished, x => x.Status!);
        }

        return null!;
    }

    public Task<List<CustomerQuestResponseModel>> GetByCustomerId(string id)
    {
        var entity = _customerQuestRepository.GetByCondition(x => x.CustomerId == id).ToList();
        CheckDataNotNull("CustomerQuest", entity);
        var mappedData = _mapper.Map<IEnumerable<CustomerQuestResponseModel>>(entity);
        return Task.FromResult(mappedData.ToList());
    }

    public async Task<CustomerQuestResponseModel> CreateAsync(CustomerQuestRequestModel request)
    {
        var entity = _mapper.Map<CustomerQuest>(request);

        var payment = await _paymentService.Get(entity.PaymentId, Language.vi);

        if (payment.Status == PaymentStatus.Pending.ToString() || payment.Status == PaymentStatus.Failed.ToString())
            throw new AppException("This transaction is not completed yet");
        if (!payment.IsValid) throw new AppException("Payment is not valid");

        // get quantity of the order
        var ticketQuantity = _paymentService.GetQuantityOfPayment(entity.PaymentId);

        // count number of order show in customer quest
        var numOfQuantityInCustomerQuest = _customerQuestRepository
            .GetByCondition(x => x.PaymentId == request.PaymentId).Count();

        // update isValid if the quantity of the order is greater than the number of order show in customer quest
        if (numOfQuantityInCustomerQuest >= ticketQuantity)
        {
            throw new AppException("Ticket quantity is not enough");
        }

        //check is previous quest is completed
        var previousQuest =
            _customerQuestRepository.GetByCondition(x => x.CustomerId == request.CustomerId && x.IsFinished == false);
        if (previousQuest.Any()) throw new AppException("Previous quest is not finished");

        entity.IsFinished = false;
        entity.Status = CommonStatus.Active.ToString();
        entity.BeginPoint = CalculateBeginPoint(payment.QuestId);
        entity.QuestId = payment.QuestId;
        entity.CreatedDate = CurrentDateTime();

        entity = await _customerQuestRepository.Add(entity);

        // trick to update payment isValid field
        if(numOfQuantityInCustomerQuest + 1 == ticketQuantity) await _paymentService.UpdateIsValidField(entity.PaymentId);

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

        // insert customer reward
        var customerId = entity.CustomerId;

        TryParse(entity.EndPoint, out var endPoint);
        TryParse(entity.BeginPoint, out var beginPoint);

        // calculate reward base on final point
        var percentPointRemain = endPoint / beginPoint * 100;

        var reward = new Reward
        {
            CustomerId = customerId,
            Name = "Reward " + DateTime.Now.ToString("dd/MM/yyyy"),
            Code = Guid.NewGuid(),
            PercentDiscount = 0,
            Status = CommonStatus.Active.ToString(),
            ReceivedDate = CurrentDateTime(),
            ExpiredDate = CurrentDateTime().AddDays(7)
        };

        reward.PercentDiscount = percentPointRemain switch
        {
            >= 90 => 30,
            >= 80 => 20,
            >= 70 => 10,
            _ => reward.PercentDiscount
        };
        //save reward if percent discount is greater than 0
        if (reward.PercentDiscount != 0)
            await _rewardRepository.Add(reward);

        var mappedData = _mapper.Map<CustomerQuestResponseModel>(entity);

        mappedData.PercentDiscount = reward.PercentDiscount;
        mappedData.RewardCode = reward.Code;
        mappedData.PercentPointRemain = percentPointRemain;

        return _mapper.Map<CustomerQuestResponseModel>(entity);
    }

    public Task<PageList<CommentResponseModel>> ShowComments(int questId, CustomerQuestParams param)
    {
        var comments = _customerQuestRepository.GetAll().Where(x => x.QuestId == questId && x.IsFinished == true)
            .OrderByDescending(x => x.CreatedDate).AsNoTracking();

        Search(ref comments, param);

        var mappedData = _mapper.Map<IEnumerable<CommentResponseModel>>(comments);

        var commentResponseModels = mappedData.ToList();
        foreach (var comment in commentResponseModels)
        {
            var customerName = _userManager!.FindByIdAsync(comment.CustomerId).Result.UserName;
            var imagePath = _userManager.FindByIdAsync(comment.CustomerId).Result.ImagePath;

            comment.ImagePath = imagePath;
            comment.Name = customerName;
        }

        return Task.FromResult(
            PageList<CommentResponseModel>.ToPageList(commentResponseModels, param.PageNumber, param.PageSize));
    }

    public async Task<List<CommentResponseModel>> UpdateComment(int customerQuestId, CommentRequestModel request)
    {
        var comments = await _customerQuestRepository.Get(customerQuestId);

        var mappedData = _mapper.Map<CustomerQuest>(comments);

        mappedData!.FeedBack = request.FeedBack;
        mappedData.Rating = request.Rating;

        await _customerQuestRepository.UpdateFields(mappedData, x => x.FeedBack!, x => x.Rating);

        return null!;
    }

    public IQueryable<CommentResponseModel> GetMyComment( int questId, string customerId)
    {
        return GetComment(questId, customerId);
    }

    public async Task ApproveFeedback(int id)
    {
        var entity = await _customerQuestRepository.Get(id);
        entity.IsFeedbackApproved = false;

        await _customerQuestRepository.UpdateFields(entity, x => x.IsFeedbackApproved);
    }

    public async Task ForceDelete(int id, bool forceDelete)
    {
        if (forceDelete)
        {
            var entity = await _customerQuestRepository.Get(id);
            entity.IsFinished = true;
            entity.Status = CommonStatus.ForceDelete.ToString();
            entity.Rating = 5;

            var lastCustomerTask = _customerTaskRepository
                .GetByCondition(x => x.CustomerQuestId == entity.Id).OrderByDescending(x => x.CreatedDate)
                .LastOrDefaultAsync().Result;

            // update last customer task status to force delete
            lastCustomerTask!.IsFinished = true;
            lastCustomerTask.Status = CommonStatus.ForceDelete.ToString();

            //
            await _customerTaskRepository.UpdateFields(lastCustomerTask, x => x.IsFinished, x => x.Status!);

            entity.EndPoint = lastCustomerTask!.CurrentPoint.ToString(CultureInfo.InvariantCulture);

            await _customerQuestRepository.UpdateFields(entity, x => x.IsFinished, x => x.Status!,
                x => x.Rating, x => x.EndPoint!);
        }
    }

    private static void Search(ref IQueryable<CustomerQuest> entities, CustomerQuestParams param)
    {
        if (!entities.Any()) return;

        if (param.QuestId != 0)
            entities = entities.Where(x => x.QuestId == param.QuestId);
        if (param.CustomerEmail != null)
        {
            var customerId = _userManager!.FindByEmailAsync(param.CustomerEmail).Result != null
                ? _userManager.FindByEmailAsync(param.CustomerEmail).Result.Id
                : null;
            entities = entities.Where(x => x.CustomerId == customerId);
        }
        if (param.IsFinished != null)
            entities = entities.Where(x => x.IsFinished == param.IsFinished);
        if(param.IsFeedbackApproved != null)
            entities = entities.Where(x => x.IsFeedbackApproved == param.IsFeedbackApproved);
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

    private IQueryable<CommentResponseModel> GetComment(int questId, string customerId)
    {
        var comments = _customerQuestRepository.GetAll()
            .Where(x => x.QuestId == questId && x.IsFinished == true && x.CustomerId == customerId);
        var mappedData = _mapper.Map<IEnumerable<CommentResponseModel>>(comments);

        var responseModels = mappedData as CommentResponseModel[] ?? mappedData.ToArray();

        foreach (var comment in responseModels)
        {
            var customerName = _userManager!.FindByIdAsync(comment.CustomerId).Result.UserName;
            var imagePath = _userManager.FindByIdAsync(comment.CustomerId).Result.ImagePath;

            comment.ImagePath = imagePath;
            comment.Name = customerName;
        }

        return responseModels.AsQueryable();
    }
}