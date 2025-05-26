var builder = WebApplication.CreateBuilder(args);

// Add services to the container

var app = builder.Build();

app.MapGet("/", () => "Hello");

// Configure the HTTP request pipeline

app.Run();
