using EntitiesLib;
using Microsoft.EntityFrameworkCore;
using ModelAppLib;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DiceLauncherDbContext>(opt => opt.UseInMemoryDatabase("dbDice"));
builder.Services.AddSingleton<IDataManager, DataBaseLinker>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
