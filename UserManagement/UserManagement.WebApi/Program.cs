using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace UserManagement.WebApi
{
    public class Program
    {
        #region Methods 
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://localhost:4000")
                .Build();
        #endregion
    }
}
