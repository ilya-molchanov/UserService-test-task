using Microsoft.OpenApi.Models;
using TestBackend.Application.Services.Interfaces;
using TestBackend.Application.Services;
using TestBackend.ServiceLibrary;
using BackendTestWebAPI.Application.Services;
using BackendTest.WebApi.Filters.Exception;

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

builder.Services.AddSingleton<IWebSocketService, WebSocketService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(3)
};
app.UseWebSockets(webSocketOptions);

app.UseRouting();

app.MapControllers();

app.Run();
