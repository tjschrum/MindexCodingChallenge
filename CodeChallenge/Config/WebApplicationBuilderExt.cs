using CodeChallenge.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeChallenge.Config
{
    public static class WebApplicationBuilderExt
    {
        private static readonly string DB_NAME = "EmployeeDB";
        private static readonly string COMPENSATION_DB_NAME = "CompensationDB";
        public static void SetupDatabases(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<EmployeeContext>(options =>
            {
                options.UseInMemoryDatabase(DB_NAME);
            });

            builder.Services.AddDbContext<CompensationContext>(options =>
            {
                options.UseInMemoryDatabase(COMPENSATION_DB_NAME);
            });
        }
    }
}
