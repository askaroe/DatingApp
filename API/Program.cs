using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyMethod().WithOrigins("https://localhost:4200"));

app.UseAuthentication(); // checks valid token or not 
app.UseAuthorization(); // cheks what user allowed to do

app.MapControllers();

app.Run();
