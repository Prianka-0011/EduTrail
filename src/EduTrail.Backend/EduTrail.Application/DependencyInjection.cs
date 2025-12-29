using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;
using AutoMapper;
using EduTrail.Application.Tests;


namespace EduTrail.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
    
            // services.AddScoped<IApplicationSettings, ApplicationSettings>();
            
            return services;
        }
    }
}
