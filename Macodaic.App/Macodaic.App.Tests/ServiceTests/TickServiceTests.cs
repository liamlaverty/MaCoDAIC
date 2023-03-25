using Macodaic.App.Core.Models;
using Macodaic.App.Core.Services;
using Macodaic.App.Core.Services.Impl;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macodaic.App.Tests.ServiceTests
{
    // Test class for TickService
    [TestClass]
    public class TickServiceTests
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private ITickService tickService;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [TestInitialize]
        public void TestInitialize()
        {
            tickService = new TickService();
        }

        // test method to check that Ticks is zero after instantiation of TickService
        [TestMethod]
        public void TickService_Constructor_InitializesTicksToZero()
        {
            // Arrange
            // Act
            var ticks = tickService.Ticks;
            // Assert
            Assert.AreEqual(0, ticks);
        }

        // test methdo to check that after TickAllEntities has happened, Ticks has increased by one
        [TestMethod]
        public void TickService_TickAllEntities_IncreasesTicksByOne()
        {
            // Arrange
            var ticksBefore = tickService.Ticks;
            // Act
            tickService.TickAllEntities();
            var ticksAfter = tickService.Ticks;
            // Assert
            Assert.AreEqual(ticksBefore + 1, ticksAfter);
        }

        // method to test TickableEntities list is instantiated after constructor
        [TestMethod]
        public void TickService_Constructor_InitializesTickableEntitiesList()
        {
            // Arrange
            // Act
            var tickableEntities = tickService.TickableEntities;
            // Assert
            Assert.IsNotNull(tickableEntities);
        }


        // method to test VendorAgents can be registered as TickableEntities
        [TestMethod]
        public void TickService_RegisterTickableEntity_CanRegisterVendorAgent()
        {
            // Arrange
            var mockVendorAgent = GetMockVendorAgent();
            // Act
            tickService.RegisterTickableEntity(mockVendorAgent.Object);
            // Assert
            Assert.IsTrue(tickService.TickableEntities.Contains(mockVendorAgent.Object));
        }

        // method to test ConsumerAgents can be registered as TickableEntities
        [TestMethod]
        public void TickService_RegisterTickableEntity_CanRegisterConsumerAgent()
        {
            // Arrange
            var mockConsumerAgent = GetMockConsumerAgent();
            // Act
            tickService.RegisterTickableEntity(mockConsumerAgent.Object);
            // Assert
            Assert.IsTrue(tickService.TickableEntities.Contains(mockConsumerAgent.Object));
        }

        // method to test that TickAllEntities calls Tick on all registered TickableEntities
        [TestMethod]
        public void TickService_TickAllEntities_CallsTickOnAllRegisteredTickableEntities()
        {
            // Arrange
            var mockVendorAgent = GetMockVendorAgent();
            var mockConsumerAgent = GetMockConsumerAgent();
            tickService.RegisterTickableEntity(mockVendorAgent.Object);
            tickService.RegisterTickableEntity(mockConsumerAgent.Object);
            // Act
            tickService.TickAllEntities();
            // Assert
            mockVendorAgent.Verify(e => e.Tick(), Times.Once);
            mockConsumerAgent.Verify(e => e.Tick(), Times.Once);
        }


        /// <summary>
        ///  private method to get a new mock of a VendorAgent
        /// </summary>
        /// <returns></returns>
        private Mock<VendorAgent> GetMockVendorAgent()
        {
            var mock = new Mock<VendorAgent>((decimal)0.5);
            mock.Setup(e => e.Tick());
            return mock;
        }

        /// <summary>
        ///  private method to get a new mock of a consumerAgent
        /// </summary>
        /// <returns></returns>
        private Mock<ConsumerAgent> GetMockConsumerAgent()
        {
            var mock = new Mock<ConsumerAgent>();
            mock.Setup(e => e.Tick());
            return mock;
        }
    }
}
