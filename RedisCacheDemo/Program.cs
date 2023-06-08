using RedisCacheDemo.Installers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InstallerServicesInAssembly(builder.Configuration);

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
