using System.Globalization;
using System.Text;
using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Exceptions;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.EmailHelper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.Momo;
using CityDiscoverTourist.Business.Settings;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MoMo;
using Newtonsoft.Json.Linq;

namespace CityDiscoverTourist.Business.IServices.Services;

public class PaymentService : BaseService, IPaymentService
{
    private readonly IMapper _mapper;
    private readonly MomoSetting _momoSettings;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IQuestRepository _questRepository;
    private readonly IRewardRepository _rewardRepository;
    private readonly ISortHelper<Payment> _sortHelper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;

    public PaymentService(IPaymentRepository paymentRepository, IMapper mapper, ISortHelper<Payment> sortHelper,
        MomoSetting momoSettings, IRewardRepository rewardRepository, IQuestRepository questRepository, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
        _momoSettings = momoSettings;
        _rewardRepository = rewardRepository;
        _questRepository = questRepository;
        _userManager = userManager;
        _emailSender = emailSender;
    }

    public PageList<PaymentResponseModel> GetAll(PaymentParams @params, Language language)
    {
        var listAll = _paymentRepository.GetAll().Include(x => x.CustomerQuests).AsNoTracking();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);

        var mappedData = _mapper.Map<IEnumerable<PaymentResponseModel>>(sortedQuests);

        var paymentResponseModels = mappedData as PaymentResponseModel[] ?? mappedData.ToArray();

        //return quest name and description in payment
        foreach (var item in paymentResponseModels)
        {
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

        var rawHash = "accessKey=" + accessKey + "&amount=" + request.Amount + "&description=" + description + "&orderId=" +
                      request.OrderId + "&partnerCode=" + request.PartnerCode + "&requestId=" + request.RequestId + "&requestType=" + requestType;

        var crypto = new MoMoSecurity();
        var signature = crypto.SignSha256(rawHash, secretKey!);

        if (signature != momoResponse!.Signature) throw new AppException("Signature is not valid");

        if (momoResponse.ResultCode != "0") throw new AppException("Failed when update status");

        if (momoResponse.Amount != entity.TotalAmount.ToString(CultureInfo.InvariantCulture))
            throw new AppException("Amount is not valid");

        entity.Status = PaymentStatus.Success.ToString();
        await _paymentRepository.Update(entity);

        //send mail to customer when payment success
        var questName = _questRepository.Get(entity.QuestId).Result.Title;
        var customerEmail = _userManager.FindByIdAsync(entity.CustomerId).Result.Email;

        var message = "<h1>Payment Success</h1>"
                      + "<h3>Dear " + customerEmail + "</h3>"
                      + "<p>Your payment has been succeeded</p>"
                      + "<p>Your order is: " + entity.Id + "</p>"
                      + "<p>Your quest name is: " + ConvertLanguage(Language.vi, questName!) + "/ "
                      + ConvertLanguage(Language.en, questName!) + "</p>"
                      + "<p>Quantity is: " + entity.Quantity + "</p>"
                      + "<p>Your order total amount is: " + entity.TotalAmount + "</p>"
                      + "<p>Your playing code is: " + entity.Id + "</p>"
                      + "<p>Your order ticket will be invalid at " + entity.CreatedDate.AddDays(2).ToString("dd/MM/yyyy HH:mm:ss") + "</p>"
                      + "<p>Thank you for using our service</p>";

        await _emailSender.SendMailConfirmAsync(customerEmail, "Payment Information", message);

        return _mapper.Map<PaymentResponseModel>(entity);
    }

    private MomoResponseModel? ConfirmPayment(MomoRequestModel request)
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

        var response = PaymentRequest.sendConfirmPaymentRequest(url!, message.ToString());
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

    public Task<List<PaymentResponseModel>> GetByCustomerId(string customerId)
    {
        var entity = _paymentRepository.GetAll().Include(x => x.CustomerQuests);
        CheckDataNotNull("Payment", entity);
        return Task.FromResult(_mapper.Map<List<PaymentResponseModel>>(entity));
    }

    public async Task<string[]> CreateAsync(PaymentRequestModel request, Guid discountCode)
    {
        //need to check customer id or not?
        if (discountCode != Guid.Empty)
        {
            var reward = _rewardRepository.GetByCondition(x => x.Code == discountCode).FirstOrDefault();
            if (reward == null) throw new AppException("Discount code is not valid");

            if (reward.Status == CommonStatus.Inactive.ToString())
                throw new AppException("Discount code is used");

            var percentage = reward.PercentDiscount;

            var entity = _mapper.Map<Payment>(request);
            entity.RewardId = reward.Id;
            entity.Status = PaymentStatus.Pending.ToString();
            entity.TotalAmount = request.totalAmount * (100 - percentage) / 100;

            var paymentUrl = MomoPayment(request, entity.TotalAmount);

            await _paymentRepository.Add(entity);

            // invalid reward when payment is success
            reward.Status = CommonStatus.Inactive.ToString();
            await _rewardRepository.UpdateFields(reward, x => x.Status!);

            return new[] { paymentUrl, entity.Id.ToString() };
        }
        else
        {
            var entity = _mapper.Map<Payment>(request);
            entity.Status = PaymentStatus.Pending.ToString();
            entity.RewardId = null;

            var paymentUrl = MomoPayment(request, entity.TotalAmount);

            await _paymentRepository.Add(entity);

            return new[] { paymentUrl, entity.Id.ToString() };
        }
    }

    public async Task<PaymentResponseModel> InvalidOrder()
    {
        var entity = _paymentRepository.GetAll().ToList();

        foreach (var item in entity)
        {
            //if(item.IsValid) RecurringJob.AddOrUpdate(() => _paymentRepository.UpdateFields(item, x=>x.IsValid == false), Cron.Minutely());
            if (!item.IsValid) continue;
            if (item.CreatedDate.AddDays(2) >= DateTime.Now.Date) continue;
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

    private static void Search(ref IQueryable<Payment> entities, PaymentParams param)
    {
        if (!entities.Any()) return;

        if (param.PaymentMethod != null) entities = entities.Where(r => r.PaymentMethod!.Equals(param.PaymentMethod));
        if (param.CustomerId != null) entities = entities.Where(r => r.CustomerId!.Equals(param.CustomerId));
    }
}