using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using System;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/reporting-structure")]
    public class ReportingStructureController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IReportingStructureService _reportStructureService;

        public ReportingStructureController(ILogger<ReportingStructureController> logger, IReportingStructureService reportStructureService)
        {
            _logger = logger;
            _reportStructureService = reportStructureService;
        }

        [HttpGet("{id}", Name = "getReportingStructureById")]
        public IActionResult GetReportingStructureById(String id)
        {
            _logger.LogDebug($"Received Reporting Structure get request for '{id}'");

            var reportingStructure = _reportStructureService.GetReportStructureById(id);

            if (reportingStructure.Employee == null)
                return NotFound();

            return Ok(reportingStructure);
        }
    }
}
