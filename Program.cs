using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Auth;
using TodoApp.Api.DB;
using TodoApp.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
AuthConfigurator.Configure(builder);
builder.Services.AddDbContext<AppDbContext>(c =>
    c.UseSqlServer(builder.Configuration["AppDbContextConnection"]), ServiceLifetime.Scoped);
builder.Services.AddTransient<ISendEmailRequestRepository, SendEmailRequestRepository>();
builder.Services.AddTransient<IToDoCreateRepository, ToDoCreateRepository>();
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