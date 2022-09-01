
using System.Net;
using System.Net.Http;

using CodeChallenge.Models;

using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void ReportingStructure_Empty_Returns_Created()
        {
            ReportingStructure rs = new ReportingStructure();

            Assert.AreEqual(rs.Employee, null);
            Assert.AreEqual(rs.NumberOfReports, 0);
        }

        [TestMethod]
        public void GetReportingStructureById_Returns_NotFound()
        {
            // Arrange
            var employeeId = "not-valid-id";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reporting-structure/{employeeId}");
            var response = getRequestTask.Result;

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void GetReportingStructureById_NoDirectReports_Returns_Ok()
        {
            // Arrange
            var employeeId = "c0c2293d-16bd-4603-8e08-638a9d18b22c";
            var expectedFirstName = "George";
            var expectedLastName = "Harrison";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reporting-structure/{employeeId}");
            var response = getRequestTask.Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            ReportingStructure rs = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedFirstName, rs.Employee.FirstName);
            Assert.AreEqual(expectedLastName, rs.Employee.LastName);
            Assert.AreEqual(0, rs.NumberOfReports);
        }

      
        /// <summary>
        /// Tests the case where an employee has direct reports and
        /// their subordinates also have direct reports
        /// </summary>
        [TestMethod]
        public void GetReportingStructureById_MultipleLevelDirectReports_Returns_Ok()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedFirstName = "John";
            var expectedLastName = "Lennon";

            // Direct Reports
            var expectedDirectReport1_EmployeeID = "b7839309-3348-463b-a7e3-5de1c168beb3";
            var expectedDirectReport1_FirstName = "Paul";
            var expectedDirectReport1_LastName = "McCartney";

            var expectedDirectReport2_EmployeeID = "03aa1462-ffa9-4978-901b-7c001562cf6f";
            var expectedDirectReport2_FirstName = "Ringo";
            var expectedDirectReport2_LastName = "Starr";

            // 2nd-Level Subordinates 
            var expectedDR1_EmployeeID = "62c1084e-6e34-4630-93fd-9153afb65309";
            var expectedDR1_FirstName = "Pete";
            var expectedDR1_LastName = "Best";

            var expectedDR2_EmployeeID = "c0c2293d-16bd-4603-8e08-638a9d18b22c";
            var expectedDR2_FirstName = "George";
            var expectedDR2_LastName = "Harrison";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reporting-structure/{employeeId}");
            var response = getRequestTask.Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            ReportingStructure rs = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedFirstName, rs.Employee.FirstName);
            Assert.AreEqual(expectedLastName, rs.Employee.LastName);

            // Validate direct reports
            Assert.AreEqual(expectedDirectReport1_EmployeeID, rs.Employee.DirectReports[0].EmployeeId);
            Assert.AreEqual(expectedDirectReport1_FirstName, rs.Employee.DirectReports[0].FirstName);
            Assert.AreEqual(expectedDirectReport1_LastName, rs.Employee.DirectReports[0].LastName);
            Assert.AreEqual(expectedDirectReport2_EmployeeID, rs.Employee.DirectReports[1].EmployeeId);
            Assert.AreEqual(expectedDirectReport2_FirstName, rs.Employee.DirectReports[1].FirstName);
            Assert.AreEqual(expectedDirectReport2_LastName, rs.Employee.DirectReports[1].LastName);

            // Validate Subordinate data
            Assert.AreEqual(expectedDR1_EmployeeID, rs.Employee.DirectReports[1].DirectReports[0].EmployeeId);
            Assert.AreEqual(expectedDR1_FirstName, rs.Employee.DirectReports[1].DirectReports[0].FirstName);
            Assert.AreEqual(expectedDR1_LastName, rs.Employee.DirectReports[1].DirectReports[0].LastName);
            Assert.AreEqual(expectedDR2_EmployeeID, rs.Employee.DirectReports[1].DirectReports[1].EmployeeId);
            Assert.AreEqual(expectedDR2_FirstName, rs.Employee.DirectReports[1].DirectReports[1].FirstName);
            Assert.AreEqual(expectedDR2_LastName, rs.Employee.DirectReports[1].DirectReports[1].LastName);

            Assert.AreEqual(4, rs.NumberOfReports);
        }

        /// <summary>
        /// Tests the case where an employee has direct reports, but their 
        /// subordinates do not have direct reports
        /// </summary>
        [TestMethod]
        public void GetReportingStructureById_SingleLevelDirectReports_Returns_Ok()
        {
            // Arrange
            var employeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f";
            var expectedFirstName = "Ringo";
            var expectedLastName = "Starr";

            // Subordinates 
            var expectedDR1_EmployeeID = "62c1084e-6e34-4630-93fd-9153afb65309";
            var expectedDR1_FirstName = "Pete";
            var expectedDR1_LastName = "Best";

            var expectedDR2_EmployeeID = "c0c2293d-16bd-4603-8e08-638a9d18b22c";
            var expectedDR2_FirstName = "George";
            var expectedDR2_LastName = "Harrison";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reporting-structure/{employeeId}");
            var response = getRequestTask.Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            ReportingStructure rs = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedFirstName, rs.Employee.FirstName);
            Assert.AreEqual(expectedLastName, rs.Employee.LastName);

            // Validate Subordinate data
            Assert.AreEqual(expectedDR1_EmployeeID, rs.Employee.DirectReports[0].EmployeeId);
            Assert.AreEqual(expectedDR1_FirstName, rs.Employee.DirectReports[0].FirstName);
            Assert.AreEqual(expectedDR1_LastName, rs.Employee.DirectReports[0].LastName);
            Assert.AreEqual(expectedDR2_EmployeeID, rs.Employee.DirectReports[1].EmployeeId);
            Assert.AreEqual(expectedDR2_FirstName, rs.Employee.DirectReports[1].FirstName);
            Assert.AreEqual(expectedDR2_LastName, rs.Employee.DirectReports[1].LastName);

            Assert.AreEqual(2, rs.NumberOfReports);
        }
    }
}
