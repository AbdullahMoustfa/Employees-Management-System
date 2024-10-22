using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Data;
using Demo.DAL.Data.Migrations;
using Demo.DAL.Models;
using Demo.PL.Extensions;
using Demo.PL.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;

namespace Demo.PL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the DI container.
        // DI => Dependancy Injection 
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(); // Register Built-In Services Required By MVC.

            ///services.AddScoped<ApplicationDbContext>();
            ///services.AddScoped<DbContextOptions<ApplicationDbContext>>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            // services.AddScoped<IDepartmentRepoistory, DepartmentRepository>();
            // services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            //ApplicationServicesExtensions.AddApplicationServices(services);   // Static Method 

            services.AddApplicationServices();    // As a Extensions Method 

            services.AddAutoMapper(M => M.AddProfile(new MappingProfiles())); 
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();

           ///services.AddScoped<UserManager<ApplicationUser>>();
           ///services.AddScoped<SignInManager<ApplicationUser>>();
           ///services.AddScoped<RoleManager<IdentityRole>>();

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredUniqueChars = 2;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true; //@#%
                options.Password.RequireUppercase = true; 
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 5;
                
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(5);

            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/SignIn";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.AccessDeniedPath = "/Home/Error";
            });

            //services.AddAuthentication("Hamda");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Hamda";
            })
                .AddCookie("Hamda", options =>
                {
					options.LoginPath = "/Account/SignIn";
					options.ExpireTimeSpan = TimeSpan.FromDays(1);
					options.AccessDeniedPath = "/Home/Error";  
				});

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
