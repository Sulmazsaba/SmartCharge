using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartCharge.Application.Contracts.Persistence;
using SmartCharge.Infrastructure.Persistence;
using SmartCharge.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharge.Infrastructure
{
    public static  class InfrastructureServiceRegistration
    {

        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SmartChargeContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SmartChargeConnectionString")));

          
            services.AddTransient(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddTransient<IGroupRepository,GroupRepository>();
            services.AddTransient<IChargeStationRepository ,ChargeStationRepository>();
            services.AddTransient<IConnectorRepository ,ConnectorRepository>();


            return services;

        }
    }
}
