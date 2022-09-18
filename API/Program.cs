using System.Globalization;
using API.Helpers;
using Infraestructura.Data;
using Infraestructura.Data.IRepositorio;
using Infraestructura.Data.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var ConnectionString = builder.Configuration.GetConnectionString("DefaulConnetion");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(ConnectionString));

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IUnidadTrabajo, UnidadTrabajo>();

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors( x=> x.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod());

app.UseAuthorization();

app.MapControllers();

app.Run();
