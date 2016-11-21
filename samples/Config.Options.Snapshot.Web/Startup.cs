using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

/// <summary>
/// This sample demonstrates how IOptionsSnapshot will be created per request.
/// TimeOptions is bound to config.json. But it won't be recreated unless the
/// config file changes.  Hit the server and verify that the creation time is
/// unchanged between requests.  Then modify the config.json and the time will
/// be updated on the next request.
/// </summary>
namespace Config.Options.Snapshot.Web
{
    public class TimeOptions
    {
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public string Message { get; set; }
    }

    public class Controller
    {
        public readonly TimeOptions _options;

        public Controller(IOptionsSnapshot<TimeOptions> options)
        {
            _options = options.Value;
        }

        public Task DisplayTimeAsync(HttpContext context)
        {
            return context.Response.WriteAsync(_options.Message + _options.CreationTime);
        }
    }

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public void Configure(IApplicationBuilder app)
        {
            app.Run(DisplayTimeAsync);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<Controller>();
            services.Configure<TimeOptions>(Configuration.GetSection("Time"));
        }

        public Task DisplayTimeAsync(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            return context.RequestServices.GetRequiredService<Controller>().DisplayTimeAsync(context);
        }

        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
            host.Run();
        }
    }
}

