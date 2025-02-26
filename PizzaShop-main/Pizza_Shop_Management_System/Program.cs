using Microsoft.EntityFrameworkCore;
using DAL.Models;
using Pizza_Shop_Management_System.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Pizza_Shop_Management_System.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Pizza_Shop_Management_System.Controllers;
using BLL.Services;
using DAL.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Connection String 
var conn = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<PizzaShopManagementSystemContext>(q => q.UseNpgsql(conn));
#endregion

// Register Services
builder.Services.AddScoped<IJwtService,JwtService>();
builder.Services.AddScoped<UserController>();
builder.Services.AddScoped<PasswordHashing>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserAuthenticationRepository>();


#region Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(30); // for custom time span by default minutes will be 20
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
}
    ); // for session 
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
#endregion

builder.Services.AddControllersWithViews();

#region Email Service
builder.Services.AddTransient<EmailService>();
#endregion


#region Jwt bearer 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
    options.Events = new JwtBearerEvents{
        OnMessageReceived = context => {
            var accessToken = context.Request.Cookies["Jwt_Token"];
            if(!string.IsNullOrEmpty(accessToken)){
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

// Add authentication using cookies
// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//     .AddCookie(options =>
//     {
//         options.LoginPath = "/UserAuthentication/Index"; // Redirect to login if not authenticated
//         options.LogoutPath = "/UserAuthentication/Logout";
//         options.AccessDeniedPath = "/Users/Index"; // Redirect if unauthorized
//     });

builder.Services.AddAuthorization();

#endregion


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

#region Session
app.UseSession(); // for session
#endregion

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UserAuthentication}/{action=Index}/{id?}");

app.Run();
