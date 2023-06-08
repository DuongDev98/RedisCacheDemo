namespace RedisCacheDemo.Installers
{
    public static class InstallerExtensions
    {
        public static void InstallerServicesInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            //b1. Lấy ra danh sách class
            var installers = typeof(Program).Assembly.GetExportedTypes()
                .Where(c => c.IsClass && !c.IsAbstract
                && c.IsPublic && typeof(IInstallser).IsAssignableFrom(c))
                .Select(Activator.CreateInstance).Cast<IInstallser>().ToList();
            //b2. Đăng ký service
            installers.ForEach(i => i.ConfigService(services, configuration));
        }
    }
}
