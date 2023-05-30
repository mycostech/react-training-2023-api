using Microsoft.EntityFrameworkCore;
using ReactTraining2023.Data.Models;
using ReactTraining2023.Services;
using ReactTraining2023.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAppScoreService, AppScoreService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(setup =>
    setup.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Mycos AppScore API",
        Version = "v1"
    })
);

builder.Services.AddDbContext<MycosReact2023TrainingContext>(
        options => {
            options.UseSqlServer(builder.Configuration.GetConnectionString("masterDB"));
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

