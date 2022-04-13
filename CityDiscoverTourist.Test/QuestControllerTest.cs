using System;
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
                Id = new Guid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Status = "Pending",
                AvailableTime = DateTime.Now,
                EstimateTime = "120",
                QuestTypeId = 1
            });

        var controller = new QuestController(questService.Object, null);

        var result = controller.Post(new QuestRequestModel()
        {
            Name = "Test",
            Description = "Test",
            Price = 100,
            Status = "Pending",
            AvailableTime = DateTime.Now,
            EstimateTime = "120",
            QuestTypeId = 1
        });

        Assert.Equal("Test", result.Result.Data.Description);
    }

    [Fact]
    public void Test2()
    {
        var questService = new Mock<IQuestService>();
        questService.Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync(new QuestResponseModel()
            {
                Id = new Guid(),
                Name = "Test",
                Description = "TestDescription",
                Price = 100,
                Status = "Pending",
                AvailableTime = DateTime.Now,
                EstimateTime = "120",
                QuestTypeId = 1
            });

        var controller = new QuestController(questService.Object, null);

        var result = controller.Get(new Guid(), "");

        Assert.Equal("TestDescription", result.Result.Data.Description);
    }
}