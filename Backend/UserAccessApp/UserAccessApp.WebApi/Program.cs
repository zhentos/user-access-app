using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserAccessApp.Infrastructure;
using UserAccessApp.WebApi.Endpoints;
using UserAccessApp.WebApi.Extensions;
using UserAccessApp.WebApi.Infrastructure;
using UserAccessApp.WebApi.Mapping;
using UserAccessApp.WebApi.Middlewares;
using UserAccessApp.WebApi.Repositories;
using UserAccessApp.WebApi.Validators;

var builder = WebApplication.CreateBuilder(args);
builder.RegisterSerilog();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);

builder.Services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuth();

builder.Services.AddSingleton<TokenProvider>();
builder.Services.AddTransient<IUserRepository>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<UserRepository>>();
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new UserRepository(logger, configuration);
});
builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();

builder.Services.AddCors(options =>
{
    var origin = builder.Configuration["AllowedOrigin"];

    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins(origin) // hardcoded for simplicity, can be obtained from config
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

// Define route groups for users and auth
var apiGroup = app.MapGroup("/api");

// Configure Users API
apiGroup.ConfigureUsersApi();

// Configure Auth API
apiGroup.ConfigureAuthApi();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");

app.Run();



