using System;
using AutoMapper;
using CityDiscoverTourist.API.Controllers;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.IServices;
using Moq;
using Xunit;

namespace CityDiscoverTourist.Test;

public class QuestControllerTest
{
    [Fact]
    public void Test1()
    {
        var questService = new Mock<IQuestService>();
        questService.Setup(x => x.CreateAsync(It.IsAny<QuestRequestModel>()))
            .ReturnsAsync((QuestRequestModel request) => new QuestResponseModel()
            {
                Id = Guid.Empty,
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                Status = "Pending",
                AvailableTime = DateTime.Now,
                EstimatedTime = "120",
                QuestTypeId = 1
            });
        var mapperMock = new Mock<IMapper>();

        var controller = new QuestController(questService.Object);

        var result = controller.Post(new QuestRequestModel()
        {
            Title = "Test",
            Description = "Test",
            Price = 100,
            Status = "Pending",
            AvailableTime = DateTime.Now,
            EstimatedTime = "120",
            QuestTypeId = 1
        });

        Assert.Equal("Test", result.Result.Data.Description);
    }

    [Fact]
    public void Test2()
    {
        var questService = new Mock<IQuestService>();
        questService.Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(new QuestResponseModel()
            {
                Id = Guid.Empty,
                Title = "Test",
                Description = "TestDescription",
                Price = 100,
                Status = "Pending",
                AvailableTime = DateTime.Now,
                EstimatedTime = "120",
                QuestTypeId = 1
            });

        var mapperMock = new Mock<IMapper>();

        var controller = new QuestController(questService.Object);

        var result = controller.Get(1);

        Assert.Equal("TestDescription", result.Result.Data.Description);
    }
}