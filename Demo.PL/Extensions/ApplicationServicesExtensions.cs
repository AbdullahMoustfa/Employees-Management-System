using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.PL.Helpers.Services.EmailSender;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.PL.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddScoped<IDepartmentRepoistory, DepartmentRepository>();
            // services.AddSingleton<IDepartmentRepoistory, DepartmentRepository>();
            // services.AddTransient<IDepartmentRepoistory, DepartmentRepository>();

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        }
    }
}
