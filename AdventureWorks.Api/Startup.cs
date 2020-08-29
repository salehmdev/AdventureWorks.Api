using System.Data;
using AdventureWorks.Commands;
using AdventureWorks.Commands.Department;
using AdventureWorks.Query.Department;
using FluentValidation.AspNetCore;
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
            services.AddControllers()
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<DepartmentCreate.CommandValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<DepartmentGetById.QueryValidator>();
                });

            services.AddScoped<IDbConnection>(provider => 
                new SqlConnection(Configuration.GetConnectionString("AdventureWorksDb")));

            services.AddDbContext<AdventureWorksContext>((provider, options) =>
            {
                options.UseSqlServer((provider.GetService<IDbConnection>() as SqlConnection)!);
            });

            services.AddMediatR(
                typeof(DepartmentGetById.QueryHandler).Assembly,
                typeof(DepartmentCreate.CommandHandler).Assembly);
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
