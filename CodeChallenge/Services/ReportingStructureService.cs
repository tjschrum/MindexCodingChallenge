using System;
using System.Collections.Generic;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public ReportingStructureService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        private Employee GetByIdWithDirectReports(String id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetByIdWithDirectReports(id);
            }

            return null;
        }

        public ReportingStructure GetReportStructureById(String id)
        {
            Employee employee = _employeeRepository.GetByIdWithDirectReports(id);

            ReportingStructure reportingStructure = new ReportingStructure()
            {
                Employee = employee,
                NumberOfReports = CountDirectReports(employee, new Dictionary<String, int>())
            };

            return reportingStructure;
        }

        /// <summary>
        /// Counts the number of direct reports for a given employee
        /// Note that if an employee's direct reports also have direct reports,
        /// they will be counted as well.
        /// </summary>
        /// <param name="e"></param> Employee to be operated on. 
        /// <param name="visitedEmployees"></param> HashTable of employees visited, used for quick look ups
        /// <param name="total"></param> The total number of direct reports
        /// <returns></returns>
        private int CountDirectReports(Employee e, Dictionary<String, int> visitedEmployees, int total = 0)
        {
            if(e == null || e.DirectReports.Count == 0){
                return 0;
            }
            // Employee has not been processed yet
            else if(!visitedEmployees.ContainsKey(e.EmployeeId))
            {
                visitedEmployees.Add(e.EmployeeId, e.DirectReports.Count);
                total += e.DirectReports.Count;
            }

            // Direct reports of current employee
            foreach(Employee employee in e.DirectReports)
            {
                Employee nextEmployee = _employeeRepository.GetByIdWithDirectReports(employee.EmployeeId);
                visitedEmployees.Add(employee.EmployeeId, employee.DirectReports.Count);
                total += CountDirectReports(nextEmployee, visitedEmployees, total);
            }

            return total;

        }
    }
}
