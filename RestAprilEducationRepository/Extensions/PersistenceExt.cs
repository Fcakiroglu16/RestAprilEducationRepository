using RestAprilEducationRepository.Application;
using RestAprilEducationRepository.Persistence;

namespace RestAprilEducationRepository.API.Extensions;

public static class PersistenceExt
{
    public static IServiceCollection AddRepositoriesExt(this IServiceCollection services)
    {
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

        return services;
    }
}