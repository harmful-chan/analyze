using analyze.core.api.Contexts;
using analyze.core.api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddLog4Net("log4net.config");

// Add services to the container.

builder.Services.AddControllers();
string? connection = builder.Configuration.GetConnectionString("AnalyzeContext");
builder.Services.AddDbContext<AnalyzeContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<AnalyzeContext>();
    context.Database.EnsureCreated();
    new DbInitializer().Initialize(context);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
