using Microsoft.Extensions.DependencyInjection;

namespace Ptg.DataAccess
{
    public static class DataAccessDependencyLoader
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IRepository, RepositoryInMemory>();
        }
    }
}
