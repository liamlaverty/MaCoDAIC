using Microsoft.VisualStudio.TestTools.UnitTesting;
using Macodaic.App.Core.Models;


namespace Macodaic.App.Tests
{
    [TestClass]
    public class AgentTests
    {
        // Test if the Agent class is abstract
        // 
        [TestMethod]
        public void Agent_IsNotAbstract()
        {
            // Arrange
            var agent = new Agent();
            // Act
            var isAbstract = agent.GetType().IsAbstract;
            // Assert
            Assert.IsFalse(isAbstract);
        }


        
    }
}