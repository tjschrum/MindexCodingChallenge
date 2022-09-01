
using System;
using System.Net;
using System.Net.Http;
using System.Text;

using CodeChallenge.Models;

using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
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
        public void CreateCompensation_Returns_Created()
        {
            // Arrange
            var employee = new Employee()
            {
                Department = "Sales",
                FirstName = "Steven",
                LastName = "Tester",
                Position = "Software Tester",
                EmployeeId = Guid.NewGuid().ToString()
            };

            var requestContent = new JsonSerialization().ToJson(employee);

            // Create new employee
            var postRequestTask = _httpClient.PostAsync("api/employee",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Employee created successfully
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newEmployee = response.DeserializeContent<Employee>();

            Compensation compensation = new Compensation()
            {
                Employee = employee,
                Salary = 10000.00m,
                EffectiveDate = DateTime.Now.Date,
                EmployeeId = newEmployee.EmployeeId
            };

            var compensationRequestBody = new JsonSerialization().ToJson(compensation);

            var postCompensationTask = _httpClient.PostAsync("api/compensation",
               new StringContent(compensationRequestBody, Encoding.UTF8, "application/json"));
            var compensationResponse = postCompensationTask.Result;

            var newCompensation = compensationResponse.DeserializeContent<Compensation>();
           
            // Validate Employee objects are equal
            Assert.AreEqual(newCompensation.Employee.Department, newEmployee.Department);
            Assert.AreEqual(newCompensation.Employee.FirstName, newEmployee.FirstName);
            Assert.AreEqual(newCompensation.Employee.LastName, newEmployee.LastName);
            Assert.AreEqual(newCompensation.Employee.Position, newEmployee.Position);

            // Validate Compensation
            Assert.AreEqual(newCompensation.Salary, compensation.Salary);
            Assert.AreEqual(newCompensation.EffectiveDate, compensation.EffectiveDate);
            Assert.AreEqual(newCompensation.EmployeeId, compensation.EmployeeId);
        }

        [TestMethod]
        public void GetCompensationById_Returns_Ok()
        {
            // Arrange
            var employee = new Employee()
            {
                Department = "Sales",
                FirstName = "Steven",
                LastName = "Tester",
                Position = "Software Tester",
                EmployeeId = "123"
            };

            var requestContent = new JsonSerialization().ToJson(employee);

            // Create new employee
            var postRequestTask = _httpClient.PostAsync("api/employee",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Employee created successfully
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newEmployee = response.DeserializeContent<Employee>();

            Compensation compensation = new Compensation()
            {
                Employee = employee,
                Salary = 10000.00m,
                EffectiveDate = DateTime.Now.Date,
                EmployeeId = newEmployee.EmployeeId
            };

            var compensationRequestContact = new JsonSerialization().ToJson(compensation);

            var postCompensationTask = _httpClient.PostAsync("api/compensation",
               new StringContent(compensationRequestContact, Encoding.UTF8, "application/json"));
            var compensationResponse = postCompensationTask.Result;

            // Validate compensation created
            Assert.AreEqual(HttpStatusCode.Created, compensationResponse.StatusCode);

            var getRequestTask = _httpClient.GetAsync($"api/compensation/{newEmployee.EmployeeId}");
            var getResponse = getRequestTask.Result;
            var newCompensation = getResponse.DeserializeContent<Compensation>();

            // Validate Employee objects are equal
            Assert.AreEqual(newCompensation.EmployeeId, newEmployee.EmployeeId);
            Assert.AreEqual(newCompensation.Employee.Department, newEmployee.Department);
            Assert.AreEqual(newCompensation.Employee.FirstName, newEmployee.FirstName);
            Assert.AreEqual(newCompensation.Employee.LastName, newEmployee.LastName);
            Assert.AreEqual(newCompensation.Employee.Position, newEmployee.Position);

            // Validate Compensation
            Assert.AreEqual(newCompensation.Salary, compensation.Salary);
            Assert.AreEqual(newCompensation.EffectiveDate, compensation.EffectiveDate);
            Assert.AreEqual(newCompensation.EmployeeId, compensation.EmployeeId);
        }
    }
}
