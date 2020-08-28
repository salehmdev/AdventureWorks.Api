using AdventureWorks.SqlData;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorks.Commands
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