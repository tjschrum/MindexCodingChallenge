using System;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.Repositories
{
    public class CompensationRespository : ICompensationRepository
    {
        private readonly CompensationContext _compensationContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public CompensationRespository(ILogger<IEmployeeRepository> logger, CompensationContext compensationContext)
        {
            _compensationContext = compensationContext;
            _logger = logger;
        }

        public Compensation Add(Compensation compensation)
        {
            // EmployeeId is required as it is the primary key
            if(compensation != null && compensation.EmployeeId != null)
            {
                _compensationContext.Add(compensation);
            }

            return compensation;
        }

        public Compensation GetById(String id)
        {
            // Include employee information when obtaining compensation
            return _compensationContext.Compensation.Include(e => e.Employee).FirstOrDefault(c => c.EmployeeId == id);
        }

        public Task SaveAsync()
        {
            return _compensationContext.SaveChangesAsync();
        }
    }
}
