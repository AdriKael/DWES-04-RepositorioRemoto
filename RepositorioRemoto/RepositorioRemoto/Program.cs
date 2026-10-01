using Microsoft.Extensions.DependencyInjection;
using RepositorioRemoto.Cache;
using RepositorioRemoto.Cache.Common;

var services = new ServiceCollection();

switch (AppConfig.Profile) {
    case "dev":
        services.AddDistributedMemoryCache();
        break;
    case "prod":
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = AppConfig.RedisConnection;
            options.InstanceName = "Users_";
        });
        break;
}

services.AddScoped<IUserCache, UserCache>();

var serviceProvider = services.BuildServiceProvider();