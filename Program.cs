using FinancialAppMvc.Actions;
using FinancialAppMvc.Contracts;
using FinancialAppMvc.Factories;
using FinancialAppMvc.Logging;
using FinancialAppMvc.Middlewares;
using FinancialAppMvc.Repositories;
using FinancialAppMvc.Services;
using Hangfire;
using Hangfire.MemoryStorage;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add config sqlite database
builder.Services.AddDbContext<FinancialAppMvc.Data.AppDbContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add config authentication
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<FinancialAppMvc.Data.AppDbContext>()
    .AddDefaultTokenProviders();

// Register repositories and actions
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionAction, TransactionAction>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserAction>();
builder.Services.AddScoped<AuditLogRepository>();

// Register services singleton
builder.Services.AddSingleton<EmailService>();
builder.Services.AddSingleton<BalanceStrategyFactory>();

// register middlewares
builder.Services.AddScoped<RedirectAuthenticateUserFilter>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// register Handfire
builder.Services.AddHangfire(config => config.UseMemoryStorage());
builder.Services.AddHangfireServer();

// register UserContextService
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContextService, UserContextService>();

// register MediatR to event listener
builder.Services.AddMediatR(typeof(Program).Assembly);

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MonitoringBehavior<,>));

// Add config authentication routes
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
});

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

app.UseAuthentication();
app.UseAuthorization();

// configure hangfire dashboard
app.UseHangfireDashboard();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
