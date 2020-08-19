using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Strade.Data.DbContexts;
using Strade.Data.Entities;
using Strade.Web.Services;

namespace Strade.Web
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
            services.AddDbContextPool<AuthenticationDbContext>(options => 
            options.UseSqlServer(Configuration.GetConnectionString("Auth"), 
            sqlServerOption => sqlServerOption.MigrationsAssembly("Strade.Data")));

            services.AddDbContextPool<ApplicationDbContext>(options => 
            options.UseSqlServer(Configuration.GetConnectionString("App"), 
            sqlServerOption => sqlServerOption.MigrationsAssembly("Strade.Data")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AuthenticationDbContext>()
            .AddDefaultTokenProviders();

            services.AddTransient<IAccountsService, AccountsService>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider svp)
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
                    pattern: "{controller=Accounts}/{action=Index}/{id?}");
            });

            MigrateDbContext(svp);
            CreateDefaultUser(svp).GetAwaiter().GetResult();
        }

        public void MigrateDbContext(IServiceProvider svp)
        {
            var authDb = svp.GetRequiredService<AuthenticationDbContext>();
            authDb.Database.Migrate();

            var appDb = svp.GetRequiredService<ApplicationDbContext>();
            appDb.Database.Migrate();
        }

        public async Task CreateDefaultUser(IServiceProvider svp)
        {
            var userEmail = "student25200020@school.com";
            var userPassword = "SuperSecretPassword@2020";
            var matNo = "25200020";

            var userManager = svp.GetRequiredService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByEmailAsync(userEmail);
            if(user is null)
            {
                user = new ApplicationUser
                {
                    Email = userEmail,
                    UserName = matNo,
                    EmailConfirmed = true,
                    PhoneNumber = "+23412345678",
                    PhoneNumberConfirmed = true,
                    MatricNo = matNo
                };

                await userManager.CreateAsync(user, userPassword);
            }
        }
    }
}
