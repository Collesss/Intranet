using Intranet.AutoMapperProfiles;
using Intranet.Repository.ActiveDirectory.AutoMapperProfiles;
using Intranet.Repository.ActiveDirectory.Implementaions;
using Intranet.Repository.ActiveDirectory.Options;
using Intranet.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.Negotiate;

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

            builder.Services.Configure<ActiveDirectoryOption>(builder.Configuration.GetSection("ActiveDirectoryOption"));

            builder.Services.AddAutoMapper(cfg => { 
                cfg.AddProfile<ActiveDirectoryAutoMapperProfile>();
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
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}