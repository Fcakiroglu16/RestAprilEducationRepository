using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestAprilEducationRepository.Application;
using RestAprilEducationRepository.Application.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestAprilEducationRepository.Persistence
{
    public static class PersistenceExt
    {
        public static void AddPersistenceExt(this IServiceCollection services, IConfiguration configuration)
        {
            //built-in => Action,Predicate,Func


            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServer"),
                    sqlServerOptionAction =>
                    {
                        sqlServerOptionAction.MigrationsAssembly(
                            typeof(PersistenceAssembly).Assembly.GetName().Name);
                    });
            });

            var applicationAssembly = typeof(ApplicationAssembly).Assembly;
            var persistenceAssembly = typeof(PersistenceAssembly).Assembly;

            var repositoryInterfaces = applicationAssembly.GetTypes()
                .Where(t => t.IsInterface && t.Name.EndsWith("Repository"));

            foreach (var serviceType in repositoryInterfaces)
            {
                var implementationType = persistenceAssembly.GetTypes()
                    .FirstOrDefault(t => t.IsClass && !t.IsAbstract && serviceType.IsAssignableFrom(t));

                if (implementationType is not null)
                    services.AddScoped(serviceType, implementationType);
            }

            //services.AddScoped<IProductRepository, ProductRepositoryWithInMemory>();
        }
    }
}
