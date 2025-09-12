using FluentValidation;
using Microsoft.EntityFrameworkCore;


using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Core.Helpers;
using BulgarianMountainTrails.Core.Interfaces;
using BulgarianMountainTrails.Core.Services;
using BulgarianMountainTrails.Core.Validations;

using BulgarianMountainTrails.Data;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddScoped<ITrailService, TrailService>();
builder.Services.AddScoped<IHutService, HutService>();
builder.Services.AddScoped<ITrailHutService, TrailHutService>();

// Validators
builder.Services.AddScoped<IValidator<TrailDto>, TrailDtoValidator>();
builder.Services.AddScoped<IValidator<HutDto>, HutDtoValidator>();

// AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile(typeof(AppProfile)));

// DbContext
builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

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
