using Business.Interfaces;
using Business.Services;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebApp_ASP.Services;

namespace WebApp_ASP;

public class Program
{
    public static void Main(string[] args)
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
        };

        var builder = WebApplication.CreateBuilder(args);

        // Connection strings
        builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DATAConnection")));

        // Identity
        builder.Services.AddIdentity<MemberEntity, IdentityRole>(o =>
            {
                o.SignIn.RequireConfirmedAccount = false;
                o.User.RequireUniqueEmail = true;
                o.Password.RequiredLength = 8;
            })
            //.AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Login/SignIn";
            options.SlidingExpiration = true;
        });

        // Add authorization services
        builder.Services.AddAuthorization();
        // Add controllers
        builder.Services.AddControllersWithViews();


        // Repositories
        builder.Services.AddScoped<IAddressRepository, AddressRepository>();
        builder.Services.AddScoped<IClientRepository, ClientRepository>();
        builder.Services.AddScoped<IMemberRepository, MemberRepository>();
        builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

        // Services
        builder.Services.AddScoped<IAddressService, AddressService>();
        builder.Services.AddScoped<IClientService, ClientService>();
        builder.Services.AddScoped<IMemberService, MemberService>();
        builder.Services.AddScoped<IProjectService, ProjectService>();
        builder.Services.AddScoped<IImageService, ImageService>();
        builder.Services.AddScoped<IAuthService, AuthService>();



        var app = builder.Build();
        app.UseHsts();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        //app.MapRazorPages()
        //   .WithStaticAssets();

        app.Run();
    }
}
