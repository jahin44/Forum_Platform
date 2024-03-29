using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using DevSkill.Http.Emails.BusinessObjects;
using System.Configuration;
using DevSkill.Http.Emails;
using Microsoft.AspNetCore.Authorization;
using Forum.Web;
using Forum.Membership;
using Forum.Membership.Contexts;
using Forum.Membership.Entities;
using Forum.Membership.Services;
using Forum.Membership.BusinessObjects;
using Forum.Membership.Seeds;
using Forum.System.Contexts;
using Forum.System;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var migrationAssemblyName = typeof(Program).Assembly.FullName;

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new WebModule());
    containerBuilder.RegisterModule(new SystemModule(connectionString, migrationAssemblyName));
    containerBuilder.RegisterModule(new MembershipModule(connectionString, migrationAssemblyName));
    containerBuilder.RegisterModule(new EmailMessagingModule(connectionString,
                migrationAssemblyName));
});

builder.Host.UseSerilog((ctx, lc) => lc
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(builder.Configuration));

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString,
                b => b.MigrationsAssembly(migrationAssemblyName)));

builder.Services.AddDbContext<SystemDbContext>(options =>
                options.UseSqlServer(connectionString,
                b => b.MigrationsAssembly(migrationAssemblyName)));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services
    .AddIdentity<ApplicationUser, Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddUserManager<UserManager>()
    .AddRoleManager<RoleManager>()
    .AddSignInManager<SignInManager>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddAuthentication()
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = new PathString("/Account/Login");
        options.AccessDeniedPath = new PathString("/Account/Denied");
        options.LogoutPath = new PathString("/Account/Logout");
        options.Cookie.Name = "";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
    });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Moderator", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new ModeratorRequirement());
    });

    options.AddPolicy("User", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new UserRequirement());
    });

    options.AddPolicy("GeneralPermission", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new GeneralPermissionRequirement());
    });
});

builder.Services.AddSingleton<IAuthorizationHandler, ModeratorRequirementHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, UserRequirementHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, GeneralPermissionRequirementHandler>();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<ModeratorDataSeed>();
builder.Services.AddControllersWithViews();
builder.Services.Configure<SmtpConfiguration>(builder.Configuration.GetSection("Smtp"));

try
{
    var app = builder.Build();
    app.Services.GetAutofacRoot();
    var dataSeed = new ModeratorDataSeed();
    dataSeed.Resolve(app.Services.GetAutofacRoot());
    await dataSeed.SeedUserAsync();

    Log.Information("Application Starting up");

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
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.MapRazorPages();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}