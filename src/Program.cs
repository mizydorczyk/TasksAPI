using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using System.Reflection;
using System.Text;
using TasksAPI;
using TasksAPI.Entities;
using TasksAPI.Middleware;
using TasksAPI.Services;
using FluentValidation.AspNetCore;
using TasksAPI.Models;
using TasksAPI.Authorization;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using TasksAPI.Models.Validators;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen();

// validators
builder.Services.AddScoped<IValidator<RegisterDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IValidator<ChangePasswordDto>, ChangePasswordDtoValidator>();
builder.Services.AddScoped<IValidator<CreateGroupDto>, CreateGroupDtoValidator>();
builder.Services.AddScoped<IValidator<CreateTaskDto>, CreateTaskDtoValidator>();

// authorization
builder.Services.AddAuthorization(options => { options.AddPolicy("JwtNotInBlacklist", policy => policy.AddRequirements(new JwtNotInBlacklist())); });
builder.Services.AddScoped<IAuthorizationHandler, JwtNotInBlacklistHandler>();

// logging
builder.Host.UseNLog();

// middleware
builder.Services.AddScoped<ErrorHandlingMiddleware>();

// user service
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);

// task and group services
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
    };
});

// db context
builder.Services.AddDbContext<TasksDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TasksDbConnection")));
builder.Services.AddScoped<BlacklistDrainer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "tasks-api"); });
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<TasksDbContext>();

    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        await dbContext.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occured during migration");
    }

    var drainer = scope.ServiceProvider.GetRequiredService<BlacklistDrainer>();
    drainer.Clear();
}

app.Run();