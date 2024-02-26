using DiffService;
using DiffService.src.services;

var builder = WebApplication.CreateBuilder(args);

//Prebaci ovde iz startup.cs


var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDiffService, DiffServiceImplementation>();


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
