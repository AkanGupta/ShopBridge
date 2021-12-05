using Microsoft.Extensions.DependencyInjection;
using ShopBridge.Domain.Interfaces;
using ShopBridge.Domain.Services;
using ShopBridge.Infrastructure.Context;
using ShopBridge.Infrastructure.Repositories;


namespace ShopBridge.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<ShopBridgeDbContext>();

            services.AddScoped<IInventoryItemRepository, InventoryItemRepository>();

            services.AddScoped<IInventoryItemService, InventoryItemService>();

            return services;
        }
    }
}
