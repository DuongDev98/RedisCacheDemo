using RedisCacheDemo.Configurations;
using RedisCacheDemo.Services;
using StackExchange.Redis;

namespace RedisCacheDemo.Installers
{
    public class CacheInstaller : IInstallser
    {
        public void ConfigService(IServiceCollection services, IConfiguration configuration)
        {
            var redisConfig = new RedisConfiguration();
            configuration.GetSection("RedisConfiguration").Bind(redisConfig);

            services.AddSingleton(redisConfig);

            if (!redisConfig.Enabled) return;

            services.AddStackExchangeRedisCache(options => {
                options.Configuration = redisConfig.ConnectionString;
            });

            services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConfig.ConnectionString));
            services.AddSingleton<ICacheService, CacheService>();
        }
    }
}
