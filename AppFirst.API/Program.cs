using AppFirst.API.Requirements;
using Microsoft.AspNetCore.Authorization;
using SharedLib.Configurations;
using SharedLib.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();


builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions"));

var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();

builder.Services.AddCustomTokenAuth(tokenOptions);

builder.Services.AddSingleton<IAuthorizationHandler, BirthDateRequirementHandler>(); //PolicyBased Authorization


builder.Services.AddAuthorization(configure =>
{
    configure.AddPolicy("istanbulPolicy", policy => //ClaimBased Authorization
    {
        policy.RequireClaim("city", "istanbul");
    });

    configure.AddPolicy("AgePolicy", policy => //PolicyBased Authorization
    {
        policy.Requirements.Add(new BirthDateRequirement(18));
    });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
