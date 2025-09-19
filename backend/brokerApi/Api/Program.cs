var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<Services.Broker>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
