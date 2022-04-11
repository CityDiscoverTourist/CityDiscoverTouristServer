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
        /*CreateMap<Tutor, Tutor>().ReverseMap();
        CreateMap<TutorModel, Tutor>().ReverseMap();

        CreateMap<Student, Student>().ReverseMap();
        CreateMap<StudentModel, Student>().ReverseMap();

        CreateMap<Class, Class>().ReverseMap();
        CreateMap<ClassModel, Class>().ReverseMap();

        CreateMap<Area, Area>().ReverseMap();

        CreateMap<Course, Course>().ReverseMap();
        CreateMap<CourseModel, Course>().ReverseMap();

        CreateMap<Grade, Grade>().ReverseMap();
        CreateMap<GradeModel, Grade>().ReverseMap();

        CreateMap<Feedback, Feedback>().ReverseMap();
        CreateMap<FeedbackModel, Feedback>().ReverseMap();

        CreateMap<Subject, Subject>().ReverseMap();

        CreateMap<TutorRequest, TutorRequest>().ReverseMap();*/


        /*CreateMap<School, School>().ReverseMap();
        CreateMap<SchoolModel, School>().ReverseMap();

        CreateMap<Account, Account>().ReverseMap();
        CreateMap<User, User>().ReverseMap();*/
    }
}