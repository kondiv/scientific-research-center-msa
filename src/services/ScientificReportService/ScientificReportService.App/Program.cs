using Microsoft.EntityFrameworkCore;
using ScientificReportService.App.Common;
using ScientificReportService.App.Domain.Configurations;
using ScientificReportService.App.Infrastructure;
using ScientificReportService.App.Infrastructure.CloudStorage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ScientificReportDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(options => 
    options.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.Configure<YandexCloudSettings>(builder.Configuration.GetSection("YandexCloud"));

builder.Services.AddScoped<IFileStorage, YandexCloudFileStorage>();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();