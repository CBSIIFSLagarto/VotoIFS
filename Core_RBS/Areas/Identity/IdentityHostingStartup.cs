using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Core_RBS.Areas.Identity.IdentityHostingStartup))]
namespace Core_RBS.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}