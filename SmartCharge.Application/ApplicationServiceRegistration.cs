using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SmartCharge.Application.Features;
using SmartCharge.Application.Features.Services;
using SmartCharge.Application.Models;
using SmartCharge.Application.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharge.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //services.AddTransient<IValidator<GroupForCreationDto>, GroupValidation>();
            ////looking for objects inherit from IRequestHandler and IRequest
            //services.AddMediatR(Assembly.GetExecutingAssembly());


            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient<IGroupService, GroupService>();
            services.AddScoped<IChargeStationService, ChargeStationService>();
            services.AddScoped<IConnectorService, ConnectorService>();
            return services;
        }
    }
}
