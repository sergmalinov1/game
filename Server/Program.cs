using Server.Hubs;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddScoped<IPlayerService, PlayerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.MapControllers();
//app.MapHub<NotificationHub>("/notification");
app.MapHub<GameHub>("/game");

app.Run();
