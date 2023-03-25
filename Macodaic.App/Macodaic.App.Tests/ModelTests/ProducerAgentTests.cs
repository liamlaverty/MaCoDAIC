using Macodaic.App.Core.Models;


namespace Macodaic.App.Tests.ModelTests
{

    // test class for VendorAgents
    [TestClass]
    public class VendorAgentTests
    {
        // Test if the VendorAgent class is abstract
        // 
        [TestMethod]
        public void VendorAgent_IsNotAbstract()
        {
            // Arrange
            var vendorAgent = new VendorAgent(100);
            // Act
            var isAbstract = vendorAgent.GetType().IsAbstract;
            // Assert
            Assert.IsFalse(isAbstract);
        }
        // Test if the VendorAgent class inherits from Agent
        // 
        [TestMethod]
        public void VendorAgent_InheritsFromAgent()
        {
            // Arrange
            var vendorAgent = new VendorAgent(100);
            // Act
            var inheritsFromAgent = vendorAgent.GetType().BaseType == typeof(Agent);
            // Assert
            Assert.IsTrue(inheritsFromAgent);
        }

        // tests if vendors have 100 dollars at the start of the simulation
        [TestMethod]
        public void VendorAgent_Has100DollarsAtStart()
        {
            // Arrange
            var vendorAgent = new VendorAgent(100);
            // Act
            var has100Dollars = vendorAgent.AvailableFunds == 100;
            // Assert
            Assert.IsTrue(has100Dollars);
        }
    }
}