using Macodaic.App.Core.Models;
using Macodaic.App.Core.Services;
using Macodaic.App.Core.Services.Impl;
using Moq;

namespace Macodaic.App.Tests.ServiceTests
{
    // Test class for the ReportService
    [TestClass]
    public class ReportServiceTests
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private IReportService reportService;
#pragma warning restore CS8618
        
        
        [TestInitialize]
        public void TestInitialize()
        {
            reportService = new ReportService();
        }

        /// <summary>
        ///  private method to get a new mock of a VendorAgent
        /// </summary>
        private Mock<VendorAgent> GetMockVendorAgent()
        {
            var mock = new Mock<VendorAgent>((decimal)0.5);
            mock.Setup(e => e.Report());
            return mock;
        }

        /// <summary>
        ///  private method to get a new mock of a ConsumerAgent
        /// </summary>
        private Mock<ConsumerAgent> GetMockConsumerAgent()
        {
            var mock = new Mock<ConsumerAgent>();
            mock.Setup(e => e.Report());
            return mock;
        }

        // method to test ReportableEntities list is instantiated after constructor has been called
        [TestMethod]
        public void ReportService_Constructor_InitializesReportableEntitiesList()
        {
            // Arrange
            // Act
            var reportableEntities = reportService.ReportableEntities;
            // Assert
            Assert.IsNotNull(reportableEntities);
        }


        // method to test the correct numberof reports are created
        [TestMethod]
        public void ReportService_RegisterReportableEntity_CreatesCorrectNumberOfReports()
        {
            // Arrange
            var numberOfReportEntities = 10;
            // Act
            for (int i = 0; i < numberOfReportEntities; i++)
            {
                var mock = new Mock<IReportableEntity>();
                mock.Setup(e => e.Report());
                reportService.RegisterReportableEntity(mock.Object);
            }
            // Assert
            Assert.AreEqual(numberOfReportEntities, reportService.ReportableEntities.Count);
        }

        

        // method to test VendorAgents can be registered as reportable entities
        [TestMethod]
        public void ReportService_RegisterReportableEntity_CanRegisterVendorAgent()
        {
            // Arrange
            
            Mock<VendorAgent> mock = GetMockVendorAgent();
            // Act
            reportService.RegisterReportableEntity(mock.Object);
            // Assert
            Assert.AreEqual(1, reportService.ReportableEntities.Count);
        }

        // method to test ConsumerAgents can be registered as reportable entities
        [TestMethod]
        public void ReportService_RegisterReportableEntity_CanRegisterConsumerAgent()
        {
            // Arrange
            Mock<ConsumerAgent> mock = GetMockConsumerAgent();

            mock.Setup(e => e.Report());
            // Act
            reportService.RegisterReportableEntity(mock.Object);
            // Assert
            Assert.AreEqual(1, reportService.ReportableEntities.Count);
        }

        // method to test both VendorAgents and ConsumerAgents can be registered as reportable entities at the same time
        [TestMethod]
        public void ReportService_RegisterReportableEntity_CanRegisterBothVendorAndConsumerAgents()
        {
            // Arrange
            Mock<VendorAgent> mockVendor = GetMockVendorAgent();
            Mock<ConsumerAgent> mockConsumer = GetMockConsumerAgent();
            // Act
            reportService.RegisterReportableEntity(mockVendor.Object);
            reportService.RegisterReportableEntity(mockConsumer.Object);
            // Assert
            Assert.AreEqual(2, reportService.ReportableEntities.Count);
        }
    }
}
