using System.Data;
using AdventureWorks.Commands;
using AdventureWorks.Commands.Department;
using AdventureWorks.Query.Employee;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdventureWorks.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDbConnection>(provider => 
                new SqlConnection(Configuration.GetConnectionString("AdventureWorksDb")));

            services.AddDbContext<AdventureWorksContext>((provider, options) =>
            {
                options.UseSqlServer((provider.GetService<IDbConnection>() as SqlConnection)!);
            });

            services.AddMediatR(
                typeof(EmployeeGetById.QueryHandler).Assembly,
                typeof(DepartmentCreate.CommandHandler).Assembly);

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
