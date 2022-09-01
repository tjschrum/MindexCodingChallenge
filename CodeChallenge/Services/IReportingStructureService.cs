using CodeChallenge.Models;
using System;

namespace CodeChallenge.Services
{
    public interface IReportingStructureService
    {
        ReportingStructure GetReportStructureById(String id);
    }
}
