/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CityDiscoverTourist.API.Controllers;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.IServices;
using CityDiscoverTourist.Business.IServices.Services;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace CityDiscoverTourist.Test;

public class QuestServiceTest
{
    private readonly IQuestService _questService;
    private readonly Mock<IQuestRepository> _questRepositoryMock = new Mock<IQuestRepository>();
    private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
    private readonly Mock<ISortHelper<Quest>> _sortHelperMock = new Mock<ISortHelper<Quest>>();
    private readonly Mock<ILocationRepository> _locationRepositoryMock = new Mock<ILocationRepository>();
    private readonly Mock<BlobService> _blobServiceMock = null;

    public QuestServiceTest ()
    {
        _questService = new QuestService(_questRepositoryMock.Object, _sortHelperMock.Object, _mapperMock.Object, null,
            _locationRepositoryMock.Object);
    }

    /*public  Task GetQuests_ShouldReturnQuests()
    {
        // Arrange
        var questId = 9;
        var quest = new List<Quest>()
        {
            new()
            {
                Id = questId,
                Title = "Test Quest",
                Description = "Test Quest Description",
            }
        };
        var mock = quest.BuildMock();

        _questRepositoryMock.Setup(x => x.GetQueryable()).Returns(mock);

        // Act
        var result = _questService.Get(questId);
        var id = result.Result.Id;
        // Assert
        Assert.Equal(questId, result.Result.Id);
        return Task.CompletedTask;
    }#1#

    [Fact]
    public async Task CreateQuests_ShouldCreateQuests()
    {
        // Arrange
        var questId = 9;
        var quest = new QuestRequestModel()
        {
            Id = questId,
            Title = "Test Quest",
            Description = "Test Quest Description",
        };
        var entity = _mapperMock.Object.Map<Quest>(quest);

        _questRepositoryMock.Setup(x => x.Add(entity)).ReturnsAsync(entity);

        // Act
        var result = await _questService.CreateAsync(quest);

        // Assert
        Assert.Equal(questId, result.Id);
    }
}*/