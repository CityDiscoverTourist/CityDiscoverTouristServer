using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.Data;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<QuestResponseModel, Quest>().ReverseMap();
        CreateMap<QuestRequestModel, Quest>().ReverseMap();

        CreateMap<QuestTypeRequestModel, QuestType>().ReverseMap();
        CreateMap<QuestTypeResponseModel, QuestType>().ReverseMap();

        CreateMap<QuestItemTypeRequestModel, QuestItemType>().ReverseMap();
        CreateMap<QuestItemTypeResponseModel, QuestItemType>().ReverseMap();

        CreateMap<CustomerQuestResponseModel, CustomerQuest>().ReverseMap();
        CreateMap<CustomerQuestRequestModel, CustomerQuest>().ReverseMap();

        CreateMap<QuestItemRequestModel, QuestItem>().ReverseMap();
        CreateMap<QuestItemResponseModel, QuestItem>().ReverseMap();

        CreateMap<RewardRequestModel, Reward>().ReverseMap();
        CreateMap<RewardResponseModel, Reward>().ReverseMap();

        CreateMap<CustomerTaskRequestModel, CustomerTask>().ReverseMap();
        CreateMap<CustomerTaskResponseModel, CustomerTask>().ReverseMap();

        CreateMap<CompetitionRequestModel, Competition>().ReverseMap();

        CreateMap<NoteRequestModel, Note>().ReverseMap();

        CreateMap<PaymentRequestModel, Payment>().ReverseMap();

        CreateMap<OwnerPaymentRequestModel, OwnerPayment>().ReverseMap();

        CreateMap<CityRequestModel, City>().ReverseMap();

        CreateMap<CommissionRequestModel, Commission>().ReverseMap();

        CreateMap<OwnerPaymentPeriodRm, OwnerPaymentPeriod>().ReverseMap();
        CreateMap<QuestOwnerRequestModel, QuestOwner>().ReverseMap();
        CreateMap<TransactionRequestModel, Transaction>().ReverseMap();
        CreateMap<WalletRequestModel, Wallet>().ReverseMap();
    }
}