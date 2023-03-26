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

        // test if the VendorAgent class has a constructor with one parameter
        [TestMethod]
        public void VendorAgent_HasConstructorWithOneParameter()
        {
            // Arrange
            var vendorAgent = new VendorAgent(100);
            // Act
            var constructor = vendorAgent.GetType().GetConstructor(new Type[] { typeof(decimal) });
            // Assert
            Assert.IsNotNull(constructor);
        }

        // test if a vendor agent has an ID after being created
        [TestMethod]
        public void VendorAgent_HasIDAfterCreation()
        {
            // Arrange
            var vendorAgent = new VendorAgent(100);
            // Act
            var hasID = vendorAgent.Id != Guid.Empty;
            // Assert
            Assert.IsTrue(hasID);
        }

        // test if the Id created is a unique guid
        [TestMethod]
        public void VendorAgent_HasUniqueID()
        {
            // Arrange
            var vendorAgent1 = new VendorAgent(100);
            var vendorAgent2 = new VendorAgent(100);
            // Act
            var hasUniqueID = vendorAgent1.Id != vendorAgent2.Id;
            // Assert
            Assert.IsTrue(hasUniqueID);
        }


        // method to test if purchaseOranges increases the number of oranges in the inventory
        [TestMethod]
        public void VendorAgent_PurchaseOranges_IncreasesInventory()
        {
            // Arrange
            var vendorAgent = new VendorAgent(100);
            var initialInventory = vendorAgent.OrangeInventory;
            // Act
            vendorAgent.PurchaseOrangesFromWholesaler(1);
            var finalInventory = vendorAgent.OrangeInventory;
            // Assert
            Assert.IsTrue(finalInventory > initialInventory);
        }

        // method to test if purchaseOranges decreases the amount of funds available
        [TestMethod]
        public void VendorAgent_PurchaseOranges_DecreasesFunds()
        {
            // Arrange
            var vendorAgent = new VendorAgent(100);
            var initialFunds = vendorAgent.AvailableFunds;
            // Act
            vendorAgent.PurchaseOrangesFromWholesaler(1);
            var finalFunds = vendorAgent.AvailableFunds;
            // Assert
            Assert.IsTrue(finalFunds < initialFunds);
        }

        // method to test if purchaseOranges adds the price of the oranges to the list of recent prices
        [TestMethod]
        public void VendorAgent_PurchaseOranges_AddsPriceToList()
        {
            // Arrange
            var vendorAgent = new VendorAgent(100);
            var initialListCount = vendorAgent._recentWholesalePriceOfOranges.Count;
            // Act
            vendorAgent.PurchaseOrangesFromWholesaler(1);
            var finalListCount = vendorAgent._recentWholesalePriceOfOranges.Count;
            // Assert
            Assert.IsTrue(finalListCount > initialListCount);
        }

        // method to test PurchaseOranges cannot purchase more oranges than the vendor has funds for
        [TestMethod]
        public void VendorAgent_PurchaseOranges_CannotPurchaseMoreThanFunds()
        {
            // Arrange
            var vendorAgent = new VendorAgent(100);
            var initialInventory = vendorAgent.OrangeInventory;
            // Act
            vendorAgent.PurchaseOrangesFromWholesaler(1000);
            var finalInventory = vendorAgent.OrangeInventory;
            // Assert
            Assert.IsTrue(finalInventory == initialInventory);
        }
        
    }
}