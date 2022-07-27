using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Data.Models;
using NotificationRequestModel = CityDiscoverTourist.Business.Data.RequestModel.NotificationRequestModel;

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
        CreateMap<CommentResponseModel, CustomerQuest>().ReverseMap();
        CreateMap<CommentRequestModel, CustomerQuest>().ReverseMap();

        CreateMap<QuestItemRequestModel, QuestItem>().ReverseMap();
        CreateMap<QuestItemResponseModel, QuestItem>().ReverseMap();

        CreateMap<RewardRequestModel, Reward>().ReverseMap();
        CreateMap<RewardResponseModel, Reward>().ReverseMap();

        CreateMap<CustomerTaskRequestModel, CustomerTask>().ReverseMap();
        CreateMap<CustomerTaskResponseModel, CustomerTask>().ReverseMap();

        CreateMap<NoteRequestModel, Note>().ReverseMap();
        CreateMap<NoteResponseModel, Note>().ReverseMap();

        CreateMap<PaymentRequestModel, Payment>().ReverseMap();
        CreateMap<PaymentResponseModel, Payment>().ReverseMap();

        CreateMap<OwnerPaymentRequestModel, OwnerPayment>().ReverseMap();
        CreateMap<OwnerPaymentResponseModel, OwnerPayment>().ReverseMap();

        CreateMap<CityRequestModel, City>().ReverseMap();
        CreateMap<CityResponseModel, City>().ReverseMap();

        CreateMap<CommissionRequestModel, Commission>().ReverseMap();
        CreateMap<CommissionResponseModel, Commission>().ReverseMap();

        CreateMap<CustomerAnswerRequestModel, CustomerAnswer>().ReverseMap();
        CreateMap<CustomerAnswerResponseModel, CustomerAnswer>().ReverseMap();

        CreateMap<OwnerPaymentPeriodRm, OwnerPaymentPeriod>().ReverseMap();
        CreateMap<OwnerPaymentPeriodResponseModel, OwnerPaymentPeriod>().ReverseMap();

        CreateMap<QuestOwnerRequestModel, QuestOwner>().ReverseMap();
        CreateMap<QuestOwnerResponseModel, QuestOwner>().ReverseMap();

        CreateMap<TransactionRequestModel, Transaction>().ReverseMap();
        CreateMap<TransactionResponseModel, Transaction>().ReverseMap();

        CreateMap<WalletRequestModel, Wallet>().ReverseMap();
        CreateMap<WalletResponseModel, Wallet>().ReverseMap();

        CreateMap<AreaRequestModel, Area>().ReverseMap();
        CreateMap<AreaResponseModel, Area>().ReverseMap();

        CreateMap<LocationRequestModel, Location>().ReverseMap();
        CreateMap<LocationResponseModel, Location>().ReverseMap();

        CreateMap<LocationTypeRequestModel, LocationType>().ReverseMap();
        CreateMap<LocationTypeResponseModel, LocationType>().ReverseMap();

        CreateMap<SuggestionRequestModel, Suggestion>().ReverseMap();
        CreateMap<SuggestionResponseModel, Suggestion>().ReverseMap();

        CreateMap<CustomerResponseModel, ApplicationUser>().ReverseMap();
        CreateMap<CustomerRequestModel, ApplicationUser>().ReverseMap();

        CreateMap<QuestRewardResponseModel, QuestReward>().ReverseMap();
        CreateMap<QuestRewardRequestModel, QuestReward>().ReverseMap();

        CreateMap<NotifyUserRequestModel, Notification>().ReverseMap();
    }
}