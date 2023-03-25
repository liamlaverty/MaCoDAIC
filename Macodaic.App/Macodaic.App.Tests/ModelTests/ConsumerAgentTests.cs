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

    }
}