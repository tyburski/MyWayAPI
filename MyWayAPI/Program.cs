using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyWayAPI;
using MyWayAPI.Services;
using System;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MWDbContext>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddScoped<ITokenDecoder, TokenDecoder>();
builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

DataInjector.Inject(app);

app.Run();
