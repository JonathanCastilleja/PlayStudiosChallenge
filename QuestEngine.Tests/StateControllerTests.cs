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
    public class StateControllerTests : IDisposable
    {
        private readonly StateController _controller;
        private readonly QuestDbContext _context;
        private readonly Mock<IOptions<QuestConfig>> _mockQuestConfig;

        public StateControllerTests()
        {
            var options = new DbContextOptionsBuilder<QuestDbContext>()
                .UseInMemoryDatabase(databaseName: "QuestDatabase")
                .Options;

            _context = new QuestDbContext(options);
            _context.QuestStates.Add(new QuestState { PlayerId = "player1", TotalQuestPoints = 50, LastMilestoneIndexCompleted = 1 });
            _context.SaveChanges();

            // Configure mock IOptions<QuestConfig>
            var questConfig = new QuestConfig{
                RateFromBet = 0.1,
                LevelBonusRate = 0.5,
                TotalQuestPointsToComplete = 1000,
                Milestones =new List<MilestoneConfig>{
                    new MilestoneConfig{MilestonePointsToComplete=200, ChipsAward=200},
                    new MilestoneConfig{MilestonePointsToComplete=400, ChipsAward=200},
                    new MilestoneConfig{MilestonePointsToComplete=700, ChipsAward=200}
                }
            };
            _mockQuestConfig = new Mock<IOptions<QuestConfig>>();
            _mockQuestConfig.Setup(q => q.Value).Returns(questConfig);

            _controller = new StateController(_mockQuestConfig.Object, _context);
        }
        public void Dispose()
        {
            // Clean up the database after each test
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public void GetQuestState_ReturnsOk_WhenPlayerIdExists()
        {
            // Act
            var result = _controller.Get("player1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var questState = Assert.IsType<GetStateResponse>(okResult.Value);
            Assert.Equal(5, questState.TotalQuestPercentCompleted);
            Assert.Equal(1, questState.LastMilestoneIndexCompleted);
        }

        [Fact]
        public void GetQuestState_ReturnsNotFound_WhenPlayerIdDoesNotExist()
        {
            // Act
            var result = _controller.Get("nonexistent_player");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
