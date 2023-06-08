namespace RedisCacheDemo.Installers
{
    public interface IInstallser
    {
        void ConfigService(IServiceCollection services, IConfiguration configuration);
    }
}