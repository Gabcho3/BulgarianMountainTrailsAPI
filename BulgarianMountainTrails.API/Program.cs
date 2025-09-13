using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Core.Helpers;
using BulgarianMountainTrails.Core.Interfaces;
using BulgarianMountainTrails.Core.Services;
using BulgarianMountainTrails.Core.Validations;
using BulgarianMountainTrails.Data;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

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
    //app.UseDeveloperExceptionPage();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";

        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception == null)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { message = "An unexpected error occurred." });
            return;
        }
        
        if(exception is ApiException apiEx)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                message = exception.Message,
                errors = apiEx.Errors
            });
            return;
        }

        context.Response.StatusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            InvalidOperationException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        await context.Response.WriteAsJsonAsync(new
        {
            message = exception.Message
        });
    });
});

app.UseHsts();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
