using Business.Interfaces;
using Business.Services;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;
using WebApp_ASP.Services;

namespace WebApp_ASP;

public class Program
{
    public static async Task Main(string[] args)
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


        // Create roles
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    IdentityRole role = new IdentityRole{ Name = roleName };
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }
        }

        //// Create default user
        //using (var scope = app.Services.CreateScope())
        //{
        //    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<MemberEntity>>();
        //    var user = new MemberEntity
        //    {
        //        DateCreated = DateOnly.FromDateTime(DateTime.Now),
        //        DateUpdated = DateOnly.FromDateTime(DateTime.Now),
        //        FirstName = "Admin",
        //        LastName = "Admin",
        //        Email = "admin@domain.com",
        //        UserName = "admin@domain.com",
        //        Title = "Admin",
        //        Status = "Active",
        //        BirthDate = DateOnly.Parse("1970-01-01"),
        //        ImageUrl = "",
        //        Id = Guid.NewGuid().ToString(),
        //        AddressId = 1,
        //        PhoneNumber = "1234567890"
        //    };

        //    var userExists = userManager.FindByEmailAsync(user.Email).Result;
        //    if(userExists == null)
        //    {
        //        var result = await userManager.CreateAsync(user, "BytMig123!").ConfigureAwait(false);
        //        if (result.Succeeded)
        //            await userManager.AddToRoleAsync(user, "Admin").ConfigureAwait(false);
        //    }

        //}

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
