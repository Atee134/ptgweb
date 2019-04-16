using Moq;
using NUnit.Framework;
using Ptg.Common.Dtos;
using Ptg.Common.Dtos.Request;
using Ptg.Common.Exceptions;
using Ptg.DataAccess;
using Ptg.HeightmapGenerator.Interfaces;
using Ptg.Services.Services;
using Ptg.SplatmapGenerator.Interfaces;
using System;

namespace Ptg.Services.Tests
{
    [TestFixture()]
    public class GameManagerServiceTests
    {
        [TestCase(TestName = "Session ID generated")]
        public void IsIdGenerated()
        {
            var gameManagerService = new GameManagerService(new Mock<IRepository>().Object);

            var result = gameManagerService.CreateGameSession();

            Assert.NotNull(result);
            Assert.AreNotEqual(Guid.Empty, result);
        }

        [TestCase(TestName = "Session ID added to repository")]
        public void IsIdStored()
        {
            var mockRepository = new Mock<IRepository>();

            var gameManagerService = new GameManagerService(mockRepository.Object);

            gameManagerService.CreateGameSession();

            mockRepository.Verify(mock => mock.AddSession(It.IsAny<Guid>()), Times.Once());
        }

        [TestCase(TestName = "Adding player adds to repository")]
        public void AddPlayerAddsToRepository()
        {
            Guid id = Guid.NewGuid();

            var mockRepository = new Mock<IRepository>();
            mockRepository.Setup(x => x.SessionExists(
                It.Is<Guid>(i => i == id))).Returns(true);

            var gameManagerService = new GameManagerService(mockRepository.Object);

            gameManagerService.AddPlayer(id, string.Empty);

            mockRepository.Verify(mock => mock.AddPlayer(It.IsAny<PlayerDto>()), Times.Once());
        }

        [TestCase(TestName = "Adding player to non existing session throws exception")]
        public void AddPlayerThrowsException()
        {
            Guid id = Guid.NewGuid();

            var mockRepository = new Mock<IRepository>();
            mockRepository.Setup(x => x.SessionExists(
                It.Is<Guid>(i => i == id))).Returns(false);

            var gameManagerService = new GameManagerService(mockRepository.Object);

            Assert.Throws<PtgNotFoundException>(() => gameManagerService.AddPlayer(id, string.Empty));
        }
    }
}
