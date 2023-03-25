using Macodaic.App.Core.Models;


namespace Macodaic.App.Tests.ModelTests
{
    // Test class for ConsumerAgent
    [TestClass]
    public class ConsumerAgentTests
    {
        // Test if the ConsumerAgent class is abstract
        // 
        [TestMethod]
        public void ConsumerAgent_IsNotAbstract()
        {
            // Arrange
            var consumerAgent = new ConsumerAgent();
            // Act
            var isAbstract = consumerAgent.GetType().IsAbstract;
            // Assert
            Assert.IsFalse(isAbstract);
        }
        // Test if the ConsumerAgent class inherits from Agent
        // 
        [TestMethod]
        public void ConsumerAgent_InheritsFromAgent()
        {
            // Arrange
            var consumerAgent = new ConsumerAgent();
            // Act
            var inheritsFromAgent = consumerAgent.GetType().BaseType == typeof(Agent);
            // Assert
            Assert.IsTrue(inheritsFromAgent);
        }

        // test if the ConsumerAgent class has a constructor with no params
        [TestMethod]
        public void ConsumerAgent_HasConstructorWithNoParameters()
        {
            // Arrange
            var consumerAgent = new ConsumerAgent();
            // Act
            var constructor = consumerAgent.GetType().GetConstructor(new Type[] { });
            // Assert
            Assert.IsNotNull(constructor);
        }

        // test if a consumer agent has an Id after being created
        [TestMethod]
        public void ConsumerAgent_HasIdAfterBeingCreated()
        {
            // Arrange
            var consumerAgent = new ConsumerAgent();
            // Act
            var hasId = consumerAgent.Id != Guid.Empty;
            // Assert
            Assert.IsTrue(hasId);
        }

        // test if a consumer agent has a unique Id after being created
        [TestMethod]
        public void ConsumerAgent_HasUniqueIdsAfterBeingCreated()
        {
            // Arrange
            var consumerAgent1 = new ConsumerAgent();
            var consumerAgent2 = new ConsumerAgent();
            // Act
            var hasUniqueIds = consumerAgent1.Id != consumerAgent2.Id;
            // Assert
            Assert.IsTrue(hasUniqueIds);
        }
    }
}