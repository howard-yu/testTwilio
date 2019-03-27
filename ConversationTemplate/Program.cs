using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using NLog.Web;

namespace ConversationTemplate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            CreateWebHostBuilder(args).Build().Run();
            //var host = BuildWebHost(args);
            //var logger = host.Services.GetRequiredService<ILogger<Program>>();
            //logger.LogInformation("Seeded the database.");

            //host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging((hostContext, logging) =>
                {
                    var env = hostContext.HostingEnvironment;
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Path.Combine(env.ContentRootPath, "Configuration"))
                        .AddJsonFile(path: "settings.json", optional: false, reloadOnChange: true)
                        .Build();
                    //logging.AddConfiguration(configuration.GetSection("Logging"));
                    logging.ClearProviders();
                    logging.AddConsole();
                });

        public static IWebHost BuildWebHost(string[] arg) =>
            WebHost.CreateDefaultBuilder(arg)
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .Build();
        }
}
