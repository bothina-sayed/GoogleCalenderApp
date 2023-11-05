using GoogleCalenderApplication.Extensions;
using GoogleCalenderApplication.Infrastructure.Context;
using GoogleCalenderApplication.Infrastructure.DependencyInjection;
using GoogleCalenderApplication.Application.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using OctoRestMaster.Extensions;
using System.Text.Json.Serialization;
using Google;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.InfrastructureStrapping();

builder.Services.ApplicationStrapping();

builder.Services.AddDbContext<ApplicationDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
sqlServerOptionsAction: options => {
    options.EnableRetryOnFailure();
    options.CommandTimeout(10);
}));

builder.Services.AddIdentityServices(builder.Configuration);


builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors(options =>
{
    options.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader();
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
