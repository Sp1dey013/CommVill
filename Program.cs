using CommVill.Controllers;
using CommVill.DAL.Helper;
using CommVill.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Logging.AddLog4Net("log4net.config");

// Add services to the container.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<CommVillDBContext>().AddDefaultTokenProviders();
builder.Services.AddControllers();
builder.Services.AddDbContext<CommVillDBContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<NVelocityHelper>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
        ClockSkew = TimeSpan.FromMinutes(5)
    };
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("Kntrol Partner",
    builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        builder.SetIsOriginAllowed((host) => true);
    });
});
builder.Services.AddLogging(options => options.AddLog4Net("log4Net.config"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.Use(async (context, next) =>
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");
        await next();
        logger.LogInformation($"Response: {context.Response.StatusCode}");
    });
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CommVillDBContext>();
    //var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    //var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await context.Database.EnsureCreatedAsync();
    await context.Database.MigrateAsync();
}

app.UseCors("Kntrol Partner");
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
