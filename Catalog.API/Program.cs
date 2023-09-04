using Catalog.API.Controllers;
using Catalog.API.ResponseModels;
using Catalog.Domain;
using Catalog.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RiskFirst.Hateoas;

namespace Catalog.API {
    public partial class Program
    {
        public static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var config = builder.Configuration;

            builder.Services.AddPersistenceServices(config);
            builder.Services.AddDomainServices();
            builder.Services.AddControllers().AddNewtonsoftJson();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            builder.Services.AddResponseCaching();

            builder.Services.AddMemoryCache();

            builder.Services.AddLinks(config =>
            {
                config.AddPolicy<ItemHateoasResponse>(policy =>
                {
                    policy.RequireRoutedLink(nameof(ItemsHateoasController.Get),
                    nameof(ItemsHateoasController.Get)).RequireRoutedLink(nameof(ItemsHateoasController.GetById),
                    nameof(ItemsHateoasController.GetById), _ => new { id = _.Data.Id })
                   .RequireRoutedLink(nameof(ItemsHateoasController.Post),
                   nameof(ItemsHateoasController.Post))
                   .RequireRoutedLink(nameof(ItemsHateoasController.Put),
                    nameof(ItemsHateoasController.Put), x => new { id = x.Data.Id })
                   .RequireRoutedLink(nameof(ItemsHateoasController.Delete),
                   nameof(ItemsHateoasController.Delete), x => new { id = x.Data.Id });
                });
            });

        var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(cfg =>
            {
                cfg.AllowAnyOrigin();
            });

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseResponseCaching();

            app.MapControllers();

            app.Run();

            return Task.CompletedTask;
        }
    }
}