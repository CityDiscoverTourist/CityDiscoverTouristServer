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


        CreateMap<RewardRequestModel, Reward>().ReverseMap();
        CreateMap<RewardResponseModel, Reward>().ReverseMap();


        CreateMap<CustomerTaskRequestModel, CustomerTask>().ReverseMap();
        CreateMap<CustomerTaskResponseModel, CustomerTask>().ReverseMap();

        CreateMap<CustomerQuestRequestModel, CustomerQuest>().ReverseMap();
        CreateMap<CustomerQuestResponseModel, CustomerQuest>().ReverseMap();

    }
}