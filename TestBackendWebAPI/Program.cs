using BackendTest.WebApi.Filters.Exception;
using Microsoft.OpenApi.Models;
using TestBackend.Application.Services.Interfaces;
using TestBackend.Application.Services;
using TestBackend.ServiceLibrary;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger Solution", Version = "v1" });
});

// DI

builder.Services.AddScoped<CustomExceptionFilter>();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.Add(new ServiceDescriptor(
    typeof(IUserService),
    typeof(UserService),
    ServiceLifetime.Scoped
));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.MapControllers();

app.Run();
