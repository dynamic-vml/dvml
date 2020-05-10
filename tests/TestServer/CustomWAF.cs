using System;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace Tests
{
    public class CustomWAF<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            try
            {
                return Host.CreateDefaultBuilder()
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            try
            {
                return base.CreateServer(builder);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
