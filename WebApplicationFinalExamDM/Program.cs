using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplicationFinalExamDM.Contexts;
using WebApplicationFinalExamDM.Helpers;
using WebApplicationFinalExamDM.Models;

namespace WebApplicationFinalExamDM
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<DbContextInitializer>();
            builder.Services.AddDbContext<AppDbContext>(option =>
            option.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            var app = builder.Build();

            var scope = app.Services.CreateScope();

            var contextInitializer = scope.ServiceProvider.GetRequiredService<DbContextInitializer>();

            await contextInitializer.InitializeDatabaseAsync();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
              name: "areas",
              pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
            );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
