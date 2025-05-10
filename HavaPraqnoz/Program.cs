using HavaPraqnoz.Services.Abstractions;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRefitClient<IWeatherClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://api.weatherapi.com/v1/"));

builder.Services.AddCors(a =>
{
    a.AddPolicy("client", builder => 
    {
        builder.WithOrigins("http://localhost", "http://client", "http://localhost:5173")
               .AllowCredentials()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
var app = builder.Build();
app.UseCors("client");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/search/{query}", async (string query, IWeatherClient weatherClient, IConfiguration configuration) =>
{
    return await weatherClient.SearchAsync(configuration["ApiKey"] ?? throw new NullReferenceException("API KEY IS NULL, PASS API KEY!"), query);
});

app.MapGet("/forecast/{id}", async (int id, IWeatherClient weatherClient, IConfiguration configuration) =>
{
    return await weatherClient.GetAsync(configuration["ApiKey"] ?? throw new NullReferenceException("API KEY IS NULL, PASS API KEY!"), id);
});
app.Run();
