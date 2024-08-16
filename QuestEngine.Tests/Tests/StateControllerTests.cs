using Microsoft.AspNetCore.Mvc;
using Moq;
using QuestEngine.WebAPI.Controllers;
using QuestEngine.WebAPI.Models;
using QuestEngine.WebAPI.Services;

namespace QuestEngine.Tests
{
    public class StateControllerTests
    {
        private readonly StateController _controller;
        private readonly Mock<IStateService> _mockStateService;

        public StateControllerTests()
        {
            _mockStateService = new Mock<IStateService>();
            _controller = new StateController(_mockStateService.Object);
        }

        [Fact]
        public void GetState_PlayerFound()
        {
            // Arrange
            var playerId = "state1";
            var expectedResponse = new GetStateResponse
            {
                TotalQuestPercentCompleted = 50.0,
                LastMilestoneIndexCompleted = 1
            };

            _mockStateService
                .Setup(s => s.GetState(playerId))
                .Returns(expectedResponse);

            // Act
            var result = _controller.Get(playerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var stateResponse = Assert.IsType<GetStateResponse>(okResult.Value);
            Assert.Equal(expectedResponse.TotalQuestPercentCompleted, stateResponse.TotalQuestPercentCompleted);
            Assert.Equal(expectedResponse.LastMilestoneIndexCompleted, stateResponse.LastMilestoneIndexCompleted);
        }

        [Fact]
        public void GetState_PlayerNotFound()
        {
            // Arrange
            var playerId = "nonexistent_player";

            _mockStateService
                .Setup(s => s.GetState(playerId))
                .Returns((GetStateResponse?)null);

            // Act
            var result = _controller.Get(playerId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}