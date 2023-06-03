using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ReactTraining2023.Data.Models;
using ReactTraining2023.Services;
using ReactTraining2023.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

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

builder.Services.AddSwaggerGen(c =>
    c.OperationFilter<AddRequiredHeaderParameter>()
);

builder.Services.AddDbContext<MycosReact2023TrainingContext>(
        options => {
            options.UseSqlServer(builder.Configuration.GetConnectionString("masterDB"));
        });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
            builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();

app.UseSwaggerUI();

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseDefaultFiles();

app.Run();

