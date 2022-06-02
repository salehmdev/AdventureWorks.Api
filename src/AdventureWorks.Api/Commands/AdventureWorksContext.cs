using AdventureWorks.Api.Data.SqlData;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorks.Api.Commands
{
    public class AdventureWorksContext : DbContext
    {
        public AdventureWorksContext(DbContextOptions<AdventureWorksContext> options)
            : base(options)
        {
        }

        public DbSet<DepartmentData> Departments { get; set; }
    }
}