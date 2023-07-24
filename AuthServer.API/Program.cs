using AuthServer.Core.Configuration;
using AuthServer.Data;
using AuthServer.Service;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Configurations;
using SharedLibrary.Extensions;
using SharedLibrary.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddFluentValidation(options =>
{
    // Validate child properties and root collection elements
    options.ImplicitlyValidateChildProperties = true;
    options.ImplicitlyValidateRootCollectionElements = true;

    // Automatic registration of validators in assembly
    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
});
// overrite InvalidModelStateResponse
builder.Services.AddCustomValidationResponseService();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDataService(builder.Configuration);
builder.Services.AddServicesService();

//binding CustomTokenOptions class with TokenOption from appSetting.Json (options pattern)
builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions"));
builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));

//Authentication operations
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();
builder.Services.AddCustomTokenAuth(tokenOptions);

#region ingredients AddCustomTokenAuth

//builder.Services.AddAuthentication(options =>
//{
//    //set Schema : "Bearer"
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    // merge two schema each other (Authentication schema and jwt schema)
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

//}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
//{
//    // validation parameters choose (valid payload)
//    // for using token options parameters
//    var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();
//    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
//    {
//        ValidIssuer = tokenOptions.Issuer,
//        ValidAudience = tokenOptions.Audiences[0], // enough 0 index for this api
//        IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

//        ValidateIssuerSigningKey = true,
//        ValidateAudience = true,
//        ValidateIssuer = true,
//        ValidateLifetime = true,
//        ClockSkew = TimeSpan.Zero // servers times diff 0

//    };

//});

#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomException();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
