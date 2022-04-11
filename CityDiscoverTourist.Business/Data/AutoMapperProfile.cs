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
    }
}