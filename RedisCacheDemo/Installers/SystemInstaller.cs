namespace RedisCacheDemo.Installers
{
    public class SystemInstaller : IInstallser
    {
        public void ConfigService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
        }
    }
}
