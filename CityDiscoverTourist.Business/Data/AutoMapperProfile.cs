using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Data.Models;
using Task = CityDiscoverTourist.Data.Models.Task;

namespace CityDiscoverTourist.Business.Data;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<QuestResponseModel, Quest>().ReverseMap();
        CreateMap<QuestRequestModel, Quest>().ReverseMap();
        CreateMap<PageList<QuestRequestModel>, Quest>().ReverseMap();

        CreateMap<QuestTypeRequestModel, QuestType>().ReverseMap();
        CreateMap<QuestTypeResponseModel, QuestType>().ReverseMap();

        CreateMap<TaskTypeRequestModel, TaskType>().ReverseMap();
        CreateMap<TaskTypeResponseModel, TaskType>().ReverseMap();

        CreateMap<TaskRequestModel, Task>().ReverseMap();
        CreateMap<TaskResponseModel, Task>().ReverseMap();

        CreateMap<RewardRequestModel, Reward>().ReverseMap();
        CreateMap<RewardResponseModel, Reward>().ReverseMap();

        CreateMap<ExperienceRequestModel, Experience>().ReverseMap();
        CreateMap<ExperienceResponseModel, Experience>().ReverseMap();
    }
}