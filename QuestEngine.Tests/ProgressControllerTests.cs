using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using QuestEngine.WebAPI.Controllers;
using QuestEngine.WebAPI.Data;
using QuestEngine.WebAPI.Models;
using System.Threading.Tasks;
using Xunit;

namespace QuestEngine.Tests{
    public class ProgressControllerTests : IDisposable
    {
        private readonly ProgressController _controller;
        private readonly QuestDbContext _context;
        private readonly Mock<IOptions<QuestConfig>> _mockQuestConfig;

        public ProgressControllerTests()
        {
            var options = new DbContextOptionsBuilder<QuestDbContext>()
                .UseInMemoryDatabase(databaseName: "QuestDatabase")
                .Options;

            _context = new QuestDbContext(options);
            _mockQuestConfig = new Mock<IOptions<QuestConfig>>();
            var questConfig = new QuestConfig{
                RateFromBet = 0.1,
                LevelBonusRate = 0.5,
                TotalQuestPointsToComplete = 1000,
                Milestones =new List<MilestoneConfig>{
                    new() {MilestonePointsToComplete=200, ChipsAward=200},
                    new() {MilestonePointsToComplete=400, ChipsAward=200},
                    new() {MilestonePointsToComplete=700, ChipsAward=200}
                }
            };
            _mockQuestConfig.Setup(q => q.Value).Returns(questConfig);
            _controller = new ProgressController(_mockQuestConfig.Object, _context);
        }
        public void Dispose()
        {
            // Clean up the database after each test
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public void PostProgress_ReturnsOk_WhenValidData()
        {
            // Arrange
            var progressData = new ProgressData
            {
                PlayerId = "player2",
                PlayerLevel = 50,
                ChipAmountBet = 3800
            };

            // Act
            var result = _controller.Post(progressData);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var questProgress = Assert.IsType<PostProgressResponse>(okResult.Value);
            Assert.Equal(405, questProgress.QuestPointsEarned);
            Assert.Equal(2, questProgress?.MilestonesCompleted?.MilestoneIndex);
            Assert.Equal(200, questProgress?.MilestonesCompleted?.ChipsAwarded);
            Assert.Equal(40.5, questProgress?.TotalQuestPercentCompleted);
        }
    }
}
