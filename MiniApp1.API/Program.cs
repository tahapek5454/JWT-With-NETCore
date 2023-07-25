using Microsoft.AspNetCore.Authorization;
using MiniApp1.API.Requirements;
using SharedLibrary.Configurations;
using SharedLibrary.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions"));
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();

builder.Services.AddCustomTokenAuth(tokenOptions);

// if you want use policy based authorization. You must write your policy
builder.Services.AddAuthorization(options =>
{
    // for example we added custom claim type was named age. Not fit arch not a role
    // so we added own policy and we define the city claim
    options.AddPolicy("AgePolicy", policy =>
    {
        // we add custom require policy
        policy.Requirements.Add(new BirthdatRequirement(18));
    });
});
// also add instance for authorizationHandler
builder.Services.AddSingleton<IAuthorizationHandler, BirthdayRequirementHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
