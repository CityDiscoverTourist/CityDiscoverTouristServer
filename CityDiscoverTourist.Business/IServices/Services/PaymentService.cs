using System.Globalization;
using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Exceptions;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.EmailHelper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.HubConfig;
using CityDiscoverTourist.Business.HubConfig.IHub;
using CityDiscoverTourist.Business.Momo;
using CityDiscoverTourist.Business.Settings;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MoMo;
using Newtonsoft.Json.Linq;

namespace CityDiscoverTourist.Business.IServices.Services;

public class PaymentService : BaseService, IPaymentService
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IEmailSender _emailSender;
    private readonly IMapper _mapper;
    private readonly MomoSetting _momoSettings;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IQuestRepository _questRepository;
    private readonly IRewardRepository _rewardRepository;
    private readonly ISortHelper<Payment> _sortHelper;
    private static  UserManager<ApplicationUser>? _userManager;
    private readonly INotificationService _notificationService;

    public PaymentService(IPaymentRepository paymentRepository, IMapper mapper, ISortHelper<Payment> sortHelper,
        MomoSetting momoSettings, IRewardRepository rewardRepository, IQuestRepository questRepository,
        UserManager<ApplicationUser>? userManager, IEmailSender emailSender,
        IBackgroundJobClient backgroundJobClient, INotificationService notificationService)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
        _momoSettings = momoSettings;
        _rewardRepository = rewardRepository;
        _questRepository = questRepository;
        _userManager = userManager;
        _emailSender = emailSender;
        _backgroundJobClient = backgroundJobClient;
        _notificationService = notificationService;
    }

    public PageList<PaymentResponseModel> GetAll(PaymentParams @params, Language language)
    {
        var listAll = _paymentRepository.GetAll().Include(x => x.CustomerQuests).OrderByDescending(x => x.CreatedDate)
            .AsNoTracking();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);

        var mappedData = _mapper.Map<IEnumerable<PaymentResponseModel>>(sortedQuests);

        var paymentResponseModels = mappedData as PaymentResponseModel[] ?? mappedData.ToArray();

        //return quest name and description in payment
        foreach (var item in paymentResponseModels)
        {
            var listCustomer = _userManager!.FindByIdAsync(item.CustomerId).Result;
            item.CustomerEmail = listCustomer.UserName;

            var listQuest = _questRepository.GetByCondition(x => x.Id == item.QuestId);

            foreach (var quest in listQuest)
            {
                item.QuestName = ConvertLanguage(language, quest.Title!);
                item.QuestDescription = ConvertLanguage(language, quest.Description!);
            }
        }

        return PageList<PaymentResponseModel>.ToPageList(paymentResponseModels, @params.PageNumber, @params.PageSize);
    }

    public async Task<PaymentResponseModel> Get(Guid id, Language language)
    {
        var entity = await _paymentRepository.Get(id);
        CheckDataNotNull("Payment", entity);

        var questId = entity.QuestId;
        var quest = await _questRepository.Get(questId);

        var mappedData = _mapper.Map<PaymentResponseModel>(entity);

        mappedData.QuestName = ConvertLanguage(language, quest.Title!);
        mappedData.QuestDescription = ConvertLanguage(language, quest.Description!);

        return mappedData;
    }

    public async Task<PaymentResponseModel> UpdateStatusWhenSuccess(MomoRequestModel request)
    {
        if (request.ResultCode != "9000") throw new AppException("Something went wrong when processing payment");

        var entity = await _paymentRepository.Get(request.OrderId);
        CheckDataNotNull("Payment", entity);

        var accessKey = _momoSettings.AccessKey;
        var secretKey = _momoSettings.SecretKey;
        var description = "";
        var requestType = "capture";
        var momoResponse = ConfirmPayment(request);

        var rawHash = "accessKey=" + accessKey + "&amount=" + request.Amount + "&description=" + description +
                      "&orderId=" + request.OrderId + "&partnerCode=" + request.PartnerCode + "&requestId=" +
                      request.RequestId + "&requestType=" + requestType;

        var crypto = new MoMoSecurity();
        var signature = crypto.SignSha256(rawHash, secretKey!);

        if (signature != momoResponse.Signature) throw new AppException("Signature is not valid");

        if (momoResponse.ResultCode != "0") throw new AppException("Failed when update status");

        if (momoResponse.Amount != entity.TotalAmount.ToString(CultureInfo.InvariantCulture))
            throw new AppException("Amount is not valid");

        entity.Status = PaymentStatus.Success.ToString();
        await _paymentRepository.Update(entity);

        //send notification to client
        var questName = _questRepository.Get(entity.QuestId).Result.Title;

        // send notification to client
        await _notificationService.CreateAsync(new NotifyUserRequestModel
        {
            Content = "New payment has been made successfully " + entity.TotalAmount + " VND" +
                      " for quest " + ConvertLanguage(Language.vi, questName!),
            CreatedDate = CurrentDateTime(),
            PaymentId = entity.Id,
            //UserId = customerId,
        });

        //send mail to customer when payment success
        var customerEmail = _userManager!.FindByIdAsync(entity.CustomerId).Result.Email;

        var message = "<h1>Payment Success</h1>" + "<h3>Dear " + customerEmail + "</h3>" +
                      "<p>Your payment has been succeeded</p>" + "<p>Your order is: " + entity.Id + "</p>" +
                      "<p>Your quest name is: " + ConvertLanguage(Language.vi, questName!) + "/ " +
                      ConvertLanguage(Language.en, questName!) + "</p>" + "<p>Quantity is: " + entity.Quantity +
                      "</p>" + "<p>Your order total amount is: " + entity.TotalAmount + "</p>" +
                      "<p>Your playing code is: " + entity.Id + "</p>" + "<p>Your order ticket will be invalid at " +
                      entity.CreatedDate.AddDays(2).ToString("dd/MM/yyyy HH:mm:ss") + "</p>" +
                      "<p>Thank you for using our service</p>";

        _backgroundJobClient.Enqueue( () =>
            _emailSender.SendMailConfirmAsync(customerEmail, "Payment Information", message));

        var mappedData = _mapper.Map<PaymentResponseModel>(entity);

        return mappedData;
    }

    public Task PushNotification(string deviceId, string userId)
    {
        var entity = _paymentRepository.GetByCondition(x => x.CustomerId == userId);

        foreach (var item in entity)
        {
            //send notification to client when payment has 1 day left to expire
            if (item.CreatedDate.AddDays(1).Date.ToString("dd/MM/yyyy HH:mm:ss") ==
                CurrentDateTime().Date.ToString("dd/MM/yyyy HH:mm:ss"))
            {
                var questName = _questRepository.Get(item.QuestId).Result.Title;

                // send notification to client
                /*_notificationService.SendNotification(new NotificationRequestModel
                {
                    DeviceId = deviceId,
                    Title = "Payment has 1 day left to expire " + item.QuestId,
                    Body = "Your payment has 1 day left to expire for quest " +
                           ConvertLanguage(Language.vi, questName!),
                    IsAndroidDevice = true
                });*/
                _backgroundJobClient.Schedule(() => _notificationService.SendNotification(new NotificationRequestModel
                {
                    DeviceId = deviceId,
                    Title = "Payment has 1 day left to expire " + item.QuestId,
                    Body = "Your payment has 1 day left to expire for quest " +
                           ConvertLanguage(Language.vi, questName!),
                    IsAndroidDevice = true
                }), TimeSpan.FromHours(24));
            }
        }
        return Task.CompletedTask;
    }

    public async Task UpdateIsValidField(Guid paymentId)
    {
        var entity = _paymentRepository.Get(paymentId).Result;
        entity.IsValid = false;
        await _paymentRepository.UpdateFields(entity, x => x.IsValid);
    }

    public async Task<PageList<PaymentResponseModel>> GetByCustomerId(PaymentParams @params, string customerId,
        Language language)
    {
        var entity = _paymentRepository.GetAll()
            .Include(x => x.CustomerQuests)
            .OrderByDescending(x => x.CreatedDate).AsNoTracking();
        CheckDataNotNull("Payment", entity);

        Search(ref entity, @params);

        var sortedQuests = _sortHelper.ApplySort(entity, @params.OrderBy);

        var mappedData = _mapper.Map<List<PaymentResponseModel>>(sortedQuests);

        foreach (var item in mappedData)
        {
            var quest = await _questRepository.Get(item.QuestId);

            item.QuestName = ConvertLanguage(language, quest.Title);
            item.QuestDescription = ConvertLanguage(language, quest.Description);
            item.ImagePath = _questRepository.Get(item.QuestId).Result.ImagePath;

            if(item.CreatedDate.AddDays(2).Date < CurrentDateTime().Date) item.IsValid = false;
        }

        return PageList<PaymentResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }

    public async Task<string[]> CreateAsync(PaymentRequestModel request, Guid discountCode, string? customerId = null)
    {
        //need to check customer id or not?
        if (discountCode != Guid.Empty)
        {
            var reward = _rewardRepository.GetByCondition(x => x.Code == discountCode).FirstOrDefault();
            if (reward == null || reward.CustomerId != request.CustomerId) throw new AppException("Discount code is not valid");

            if (reward.Status == CommonStatus.Inactive.ToString())
                throw new AppException("Discount code is used");

            var entity = _mapper.Map<Payment>(request);

            entity.RewardId = reward.Id;
            entity.Status = PaymentStatus.Pending.ToString();
            entity.TotalAmount = request.TotalAmount;
            entity.CreatedDate = CurrentDateTime();
            entity.PaymentMethod = "MomoWallet";

            var paymentUrl = MomoPayment(request, entity.TotalAmount);

            await _paymentRepository.Add(entity);

            // invalid reward when payment is success
            reward.Status = CommonStatus.Inactive.ToString();
            await _rewardRepository.UpdateFields(reward, x => x.Status!);

            // update payment when over 10 min
            _backgroundJobClient.Schedule(() => PaymentFailed(entity.Id), TimeSpan.FromMinutes(15));

            return new[] { paymentUrl, entity.Id.ToString() };
        }
        else
        {
            var entity = _mapper.Map<Payment>(request);

            entity.Status = PaymentStatus.Pending.ToString();
            entity.RewardId = null;
            entity.CreatedDate = CurrentDateTime();
            entity.PaymentMethod = "MomoWallet";

            var paymentUrl = MomoPayment(request, entity.TotalAmount);

            await _paymentRepository.Add(entity);

            // update status when over 10 min
            _backgroundJobClient.Schedule(() => PaymentFailed(entity.Id), TimeSpan.FromMinutes(15));

            return new[] { paymentUrl, entity.Id.ToString() };
        }
    }

    public Task<string[]> CheckCoupon(Guid couponCode, string customerId, float total)
    {
        var reward = _rewardRepository.GetByCondition(x => x.Code == couponCode).FirstOrDefault();
        if (reward == null || reward.CustomerId != customerId) throw new AppException("Discount code is not valid");

        if (reward.Status == CommonStatus.Inactive.ToString())
            throw new AppException("Discount code is used");

        var percentage = reward.PercentDiscount;
        var discountAmount = total * (100 - percentage) / 100;

        return Task.FromResult(new [] { percentage.ToString(), discountAmount.ToString(CultureInfo.InvariantCulture)});
    }

    public async Task<PaymentResponseModel> InvalidOrder()
    {
        var entity = _paymentRepository.GetAll().ToList();

        foreach (var item in entity)
        {
            //if(item.IsValid) RecurringJob.AddOrUpdate(() => _paymentRepository.UpdateFields(item, x=>x.IsValid == false), Cron.Minutely());
            if (!item.IsValid) continue;

            // if customer buy it on 9.30pm it will be count at 12AM that day
            // ex: 9h30PM 24/07/2022 -> 12AM 24/07/2022
            if (item.CreatedDate.AddDays(2).Date >= CurrentDateTime().Date) continue;
            item.IsValid = false;
            await _paymentRepository.UpdateFields(item, x => x.IsValid);
        }

        return null!;
    }

    public async Task<PaymentResponseModel> DeleteAsync(Guid id)
    {
        var entity = await _paymentRepository.Delete(id);
        return _mapper.Map<PaymentResponseModel>(entity);
    }

    public int GetQuantityOfPayment(Guid id)
    {
        return _paymentRepository.Get(id).Result.Quantity;
    }

    private MomoResponseModel ConfirmPayment(MomoRequestModel request)
    {
        var partnerCode = request.PartnerCode;
        var orderId = request.OrderId;
        var requestId = request.RequestId;
        var amount = request.Amount;
        var requestType = "capture";
        var accessKey = _momoSettings.AccessKey;
        var secretKey = _momoSettings.SecretKey;
        var description = "";

        var param = "accessKey=" + accessKey + "&amount=" + amount + "&description=" + description + "&orderId=" +
                    orderId + "&partnerCode=" + partnerCode + "&requestId=" + requestId + "&requestType=" + requestType;

        var crypto = new MoMoSecurity();
        var signature = crypto.SignSha256(param, secretKey!);

        var url = "https://test-payment.momo.vn/v2/gateway/api/confirm";

        var message = new JObject
        {
            { "partnerCode", partnerCode },
            { "requestId", requestId },
            { "orderId", orderId },
            { "requestType", requestType },
            { "amount", amount },
            { "lang", "en" },
            { "description", description },
            { "signature", signature}
        };

        var response = PaymentRequest.sendConfirmPaymentRequest(url, message.ToString());
        var jMessage = JObject.Parse(response);

        return new MomoResponseModel
        {
            Amount = jMessage["amount"]!.ToString(),
            Message = jMessage["message"]!.ToString(),
            OrderId = (Guid) jMessage["orderId"]!,
            PartnerCode = jMessage["partnerCode"]!.ToString(),
            RequestId = jMessage["requestId"]!.ToString(),
            ResponseTime = jMessage["responseTime"]!.ToString(),
            RequestType = jMessage["requestType"]!.ToString(),
            TransId = jMessage["transId"]!.ToString(),
            ResultCode = jMessage["resultCode"]!.ToString(),
            Signature = signature
        };
    }

    private string MomoPayment(PaymentRequestModel request, float totalAmount)
    {

        var endpoint = _momoSettings.EndPoint;
        var partnerCode = _momoSettings.PartnerCode;
        var accessKey = _momoSettings.AccessKey;
        var secretKey = _momoSettings.SecretKey;
        var orderInfo = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + request.QuestId;
        var redirectUrl = "https://www.citydiscovery.tech/thank/";
        var ipnUrl = "https://citytourist.azurewebsites.net/api/v1/payments/callback";
        var requestType = "captureWallet";

        // if mobile then change redirectUrl to open mobile app
        if (request.IsMobile) redirectUrl = "https://citydiscovertourist.page.link/homepage";

        var amount = totalAmount.ToString(CultureInfo.InvariantCulture);
        var orderId = request.Id.ToString();
        var requestId = Guid.NewGuid();
        var extraData = "";

        var rawHash = "accessKey=" + accessKey + "&amount=" + amount + "&extraData=" + extraData + "&ipnUrl=" + ipnUrl +
                      "&orderId=" + orderId + "&orderInfo=" + orderInfo + "&partnerCode=" + partnerCode +
                      "&redirectUrl=" + redirectUrl + "&requestId=" + requestId + "&requestType=" + requestType;

        var crypto = new MoMoSecurity();
        var signature = crypto.SignSha256(rawHash, secretKey!);

        var message = new JObject
        {
            { "partnerCode", partnerCode },
            { "partnerName", "City Tour" },
            { "storeId", "MomoTestStore" },
            { "requestId", requestId },
            { "amount", amount },
            { "orderId", orderId },
            { "orderInfo", orderInfo },
            { "redirectUrl", redirectUrl },
            { "ipnUrl", ipnUrl },
            { "lang", "en" },
            { "autoCapture", false},
            { "extraData", extraData },
            { "requestType", requestType },
            { "signature", signature }
        };

        var response = PaymentRequest.sendPaymentRequest(endpoint!, message.ToString());
        var jMessage = JObject.Parse(response);
        var paymentUrl = jMessage.GetValue("payUrl")!.ToString();
        return paymentUrl;
    }

    public async Task PaymentFailed(Guid paymentId)
    {
        var payment = await _paymentRepository.Get(paymentId);
        if (payment.Status == PaymentStatus.Pending.ToString())
        {
            payment.IsValid = false;
            payment.Status = PaymentStatus.Failed.ToString();
            await _paymentRepository.UpdateFields(payment, x => x.Status!);
        }
    }

    private static void Search(ref IQueryable<Payment> entities, PaymentParams param)
    {
        if (!entities.Any()) return;

        if (param.PaymentMethod != null) entities = entities.Where(r => r.PaymentMethod!.Equals(param.PaymentMethod));
        if (param.CustomerEmail != null)
        {
            var customerId = _userManager!.FindByEmailAsync(param.CustomerEmail).Result != null
                ? _userManager.FindByEmailAsync(param.CustomerEmail).Result.Id
                : null;
            entities = entities.Where(x => x.CustomerId == customerId);
        }
        if (param.Status != null) entities = entities.Where(r => r.Status!.Equals(param.Status));
        if (param.IsValid != null) entities = entities.Where(r => r.IsValid.Equals(param.IsValid));
    }
}