using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using QuestEngine.WebAPI.Controllers;
using QuestEngine.WebAPI.Data;
using QuestEngine.WebAPI.Models;
using QuestEngine.WebAPI.Services;

namespace QuestEngine.Tests
{
    public class ProgressControllerTests
    {
        private readonly ProgressController _controller;
        private readonly Mock<IOptions<QuestConfig>> _mockOptions;
        private readonly Mock<IProgressService> _mockProgressService;

        public ProgressControllerTests()
        {
            _mockProgressService = new Mock<IProgressService>();
            _mockOptions = new Mock<IOptions<QuestConfig>>();
            var questConfig = new QuestConfig
            {
                RateFromBet = 0.1,
                LevelBonusRate = 0.5,
                TotalQuestPointsToComplete = 1000,
                Milestones = new List<MilestoneConfig>
                {
                    new() { MilestonePointsToComplete = 200, ChipsAward = 200 },
                    new() { MilestonePointsToComplete = 400, ChipsAward = 200 },
                    new() { MilestonePointsToComplete = 700, ChipsAward = 200 }
                }
            };
            _mockOptions.Setup(o => o.Value).Returns(questConfig);

            var progressService = new ProgressService(_mockOptions.Object, new QuestDbContext(new DbContextOptionsBuilder<QuestDbContext>()
                .UseInMemoryDatabase(databaseName: "QuestDatabase")
                .Options));
            
            _controller = new ProgressController(progressService);
        }

        [Fact]
        public void PostProgress_AchievedMilestone()
        {
            // Arrange
            var progressData = new ProgressData
            {
                PlayerId = "progress1",
                PlayerLevel = 50,
                ChipAmountBet = 3800
            };

            var expectedResponse = new PostProgressResponse
            {
                QuestPointsEarned = 405,
                TotalQuestPercentCompleted = 40.5,
                MilestonesCompleted = new Milestone
                {
                    MilestoneIndex = 2,
                    ChipsAwarded = 200
                }
            };

            _mockProgressService
                .Setup(s => s.CalculateProgress(It.IsAny<ProgressData>()))
                .Returns(expectedResponse);

            // Act
            var result = _controller.Post(progressData);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var questProgress = Assert.IsType<PostProgressResponse>(okResult.Value);
            Assert.Equal(expectedResponse.QuestPointsEarned, questProgress.QuestPointsEarned);
            Assert.Equal(expectedResponse.MilestonesCompleted.MilestoneIndex, questProgress?.MilestonesCompleted?.MilestoneIndex);
            Assert.Equal(expectedResponse.MilestonesCompleted.ChipsAwarded, questProgress?.MilestonesCompleted?.ChipsAwarded);
            Assert.Equal(expectedResponse.TotalQuestPercentCompleted, questProgress?.TotalQuestPercentCompleted);
        }

        [Fact]
        public void PostProgress_NotAchievedMilestone()
        {
            // Arrange
            var progressData1 = new ProgressData
            {
                PlayerId = "progress2",
                PlayerLevel = 50,
                ChipAmountBet = 3800
            };
            var progressData2 = new ProgressData
            {
                PlayerId = "progress2",
                PlayerLevel = 50,
                ChipAmountBet = 1000
            };

            var expectedResponse = new PostProgressResponse
            {
                QuestPointsEarned = 125,
                TotalQuestPercentCompleted = 53.0,
                MilestonesCompleted = new Milestone
                {
                    MilestoneIndex = 2,
                    ChipsAwarded = 0
                }
            };

            _mockProgressService
                .Setup(s => s.CalculateProgress(It.IsAny<ProgressData>()))
                .Returns(expectedResponse);

            // Act
            _controller.Post(progressData1); // Initial progress
            var result = _controller.Post(progressData2); // Subsequent progress

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var questProgress = Assert.IsType<PostProgressResponse>(okResult.Value);
            Assert.Equal(expectedResponse.QuestPointsEarned, questProgress.QuestPointsEarned);
            Assert.Equal(expectedResponse.MilestonesCompleted.MilestoneIndex, questProgress?.MilestonesCompleted?.MilestoneIndex);
            Assert.Equal(expectedResponse.MilestonesCompleted.ChipsAwarded, questProgress?.MilestonesCompleted?.ChipsAwarded);
            Assert.Equal(expectedResponse.TotalQuestPercentCompleted, questProgress?.TotalQuestPercentCompleted);
        }
    }
}