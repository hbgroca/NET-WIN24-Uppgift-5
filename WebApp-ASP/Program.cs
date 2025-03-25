using Business.Interfaces;
using Business.Services;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebApp_ASP.Data;
using WebApp_ASP.Services;

namespace WebApp_ASP;

public class Program
{
    public static void Main(string[] args)
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            // Clean up json file
            WriteIndented = true,
            // Prevent infinite loop when collecting db data where JOIN can cause trubble
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
        };

        // Create a new builder
        var builder = WebApplication.CreateBuilder(args);

        

        // Connection strings
        var dataConnectionString = builder.Configuration.GetConnectionString("DATAConnection") ?? throw new InvalidOperationException("Connection string 'DATAConnection' not found.");
        builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\roban\\Desktop\\Github\\NET-WIN24-Uppgift-5\\Data\\Database\\Assignment5.mdf;Integrated Security=True;Connect Timeout=30"));
        //builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("dataConnectionString"));
        var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>options.UseSqlServer(defaultConnectionString));

        // Identity
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
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



        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Login}/{action=SignIn}/{id?}")
            .WithStaticAssets();
        app.MapRazorPages()
           .WithStaticAssets();

        app.Run();
    }
}
