using System.Globalization;
using System.Web;
using AutoMapper;
using Azure.Core;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.Momo;
using CityDiscoverTourist.Business.Settings;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace CityDiscoverTourist.Business.IServices.Services;

public class PaymentService : BaseService, IPaymentService
{
    private readonly IMapper _mapper;
    private readonly IPaymentRepository _paymentRepository;
    private readonly ISortHelper<Payment> _sortHelper;
    private readonly MomoSetting _momoSettings;

    public PaymentService(IPaymentRepository paymentRepository, IMapper mapper, ISortHelper<Payment> sortHelper, MomoSetting momoSettings)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
        _momoSettings = momoSettings;
    }

    public PageList<PaymentResponseModel> GetAll(PaymentParams @params)
    {
        var listAll = _paymentRepository.GetAll().Include(x => x.CustomerQuests).AsNoTracking();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        //var shapedData = _dataShaper.ShapeData(sortedQuests, param.Fields);
        var mappedData = _mapper.Map<IEnumerable<PaymentResponseModel>>(sortedQuests);
        return PageList<PaymentResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }

    public async Task<PaymentResponseModel> Get(Guid id)
    {
        var entity = await _paymentRepository.Get(id);
        CheckDataNotNull("Payment", entity);
        return _mapper.Map<PaymentResponseModel>(entity);
    }

    public Task<List<PaymentResponseModel>> GetByCustomerId(string customerId)
    {
        var entity = _paymentRepository.GetAll().Include(x => x.CustomerQuests);
        CheckDataNotNull("Payment", entity);
        return Task.FromResult(_mapper.Map<List<PaymentResponseModel>>(entity));
    }

    public async Task<string> CreateAsync(PaymentRequestModel request)
    {
        var entity = _mapper.Map<Payment>(request);
        entity.Status = CommonStatus.Pending.ToString();

        var paymentUrl = MomoPayment(request);

        await _paymentRepository.Add(entity);

        return paymentUrl;
    }

    private string MomoPayment(PaymentRequestModel request)
    {
        var endpoint = _momoSettings.EndPoint;
        var partnerCode = _momoSettings.PartnerCode;
        var accessKey = _momoSettings.AccessKey;
        var secretKey = _momoSettings.SecretKey;
        var orderInfo = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + request.QuestId;
        var redirectUrl = "https://www.citydiscovery.tech/thank/";
        var ipnUrl = "http://localhost:3000/data/";
        var requestType = "captureWallet";

        var amount = request.TotalAmount.ToString(CultureInfo.InvariantCulture);
        var orderId = Guid.NewGuid();
        var requestId = Guid.NewGuid();
        var extraData = "";

        var rawHash = "accessKey=" + accessKey + "&amount=" + amount + "&extraData=" + extraData + "&ipnUrl=" + ipnUrl +
                      "&orderId=" + orderId + "&orderInfo=" + orderInfo + "&partnerCode=" + partnerCode + "&redirectUrl=" +
                      redirectUrl + "&requestId=" + requestId + "&requestType=" + requestType;

        var crypto = new MoMoSecurity();
        var signature = crypto.signSHA256(rawHash, secretKey!);

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
            { "extraData", extraData },
            { "requestType", requestType },
            { "signature", signature }
        };

        var response = PaymentRequest.sendPaymentRequest(endpoint!, message.ToString());
        var jMessage = JObject.Parse(response);
        var paymentUrl = jMessage.GetValue("payUrl")!.ToString();
        return paymentUrl;
    }

    public Task<PaymentResponseModel> UpdateAsync(int id, PaymentRequestModel request)
    {
        throw new NotImplementedException();
    }

    public async Task<PaymentResponseModel> UpdateAsync(PaymentRequestModel request)
    {
        var entity = _mapper.Map<Payment>(request);
        entity = await _paymentRepository.Update(entity);
        return _mapper.Map<PaymentResponseModel>(entity);
    }

    public async Task<PaymentResponseModel> DeleteAsync(Guid id)
    {
        var entity = await _paymentRepository.Delete(id);
        return _mapper.Map<PaymentResponseModel>(entity);
    }

    private static void Search(ref IQueryable<Payment> entities, PaymentParams param)
    {
        if (!entities.Any()) return;

        if (param.PaymentMethod != null) entities = entities.Where(r => r.PaymentMethod!.Equals(param.PaymentMethod));
    }

    public int GetQuantityOfPayment(Guid id)
    {
        return _paymentRepository.Get(id).Result.Quantity;
    }
}