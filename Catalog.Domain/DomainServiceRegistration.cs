using Catalog.Domain.Contracts.Persistence;
using Catalog.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using FluentValidation.AspNetCore;
using FluentValidation;
using Catalog.Domain.DTOs.Request.Item;
using Catalog.Domain.DTOs.Request.Validators;

namespace Catalog.Domain
{
    public static class DomainServiceRegistration
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IArtistService, ArtistService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IValidator<AddItemRequest>, AddItemRequestValidator>();
            services.AddScoped<IValidator<EditItemRequest>, EditItemRequestValidator>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }

    }
}
