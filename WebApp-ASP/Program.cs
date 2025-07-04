using Business.Hubs;
using Business.Interfaces;
using Business.Services;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApp_ASP;

public class Program
{
    public static async Task Main(string[] args)
    {
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
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Login/SignIn";
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.HttpOnly = true;
            options.SlidingExpiration = true;
            options.ExpireTimeSpan = TimeSpan.FromHours(4);
        });

        // External login authentication
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        .AddCookie()
        .AddGoogle(options =>
        {
            options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
            options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            options.SignInScheme = IdentityConstants.ExternalScheme;
        });

        // Authorization services
        builder.Services.AddAuthorization();
        // Controllers
        builder.Services.AddControllersWithViews();
        // SignalR
        builder.Services.AddSignalR();
        // Cookie concent
        builder.Services.Configure<CookiePolicyOptions>(options =>
        {
            // Check if the user has already given consent
            options.CheckConsentNeeded = context => !context.Request.Cookies.ContainsKey("cookieConsent");
            options.MinimumSameSitePolicy = SameSiteMode.Lax;
        });

        // Repositories
        builder.Services.AddScoped<IAddressRepository, AddressRepository>();
        builder.Services.AddScoped<IClientRepository, ClientRepository>();
        builder.Services.AddScoped<IMemberRepository, MemberRepository>();
        builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
        builder.Services.AddScoped<INotificationsRepository, NotificationsRepository>();
        builder.Services.AddScoped<IClientStatusRepository, ClientStatusRepository>();
        builder.Services.AddScoped<IMemberStatusRepository, MemberStatusRepository>();

        // Services
        builder.Services.AddScoped<IAddressService, AddressService>();
        builder.Services.AddScoped<IClientService, ClientService>();
        builder.Services.AddScoped<IMemberService, MemberService>();
        builder.Services.AddScoped<IProjectService, ProjectService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IImageServices, ImageServices>();
        builder.Services.AddScoped<INotificationSerivces, NotificationSerivces>();
        builder.Services.AddScoped<IStatusServices, StatusServices>();

        var app = builder.Build();
        app.UseHsts();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCookiePolicy();
        app.UseAuthentication();
        app.UseAuthorization();

        // Create roles if they dont already exists
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "Member" };

            foreach (var roleName in roleNames)
            {
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    IdentityRole role = new IdentityRole{ Name = roleName };
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }
        }

        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.MapHub<NotificationHub>("/notificationhub");

        app.Run();
    }
}
