using Intranet.AutoMapperProfiles;
using Intranet.Repository.Db;
using Intranet.Repository.Db.Implementations;
using Intranet.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;

namespace Intranet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
            .AddNegotiate();

            builder.Services.AddAuthorization(options =>
            {
                // By default, all incoming requests will be authorized according to the default policy.
                options.FallbackPolicy = options.DefaultPolicy;
            });
            builder.Services.AddRazorPages();

            builder.Services.AddDbContext<RepositoryDbContext>(opts =>
                opts.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddAutoMapper(cfg => { 
                cfg.AddProfile<IntranetAutoMapperProfile>();
            });

            builder.Services.AddScoped<IUserRepository, UserRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=User}/{action=ViewUser}/{id?}");

            app.Run();
        }
    }
}