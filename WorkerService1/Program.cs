using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WorkerService1;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((hostContext, services) =>
            {
                IConfiguration configuration = hostContext.Configuration;

                // Bind SMTP settings from configuration
                services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));

                // Register SMTP settings and services
                services.AddSingleton(resolver =>
                    resolver.GetRequiredService<IOptions<SmtpSettings>>().Value);
                services.AddSingleton<IEmailService, EmailService>();
                services.AddHostedService<Worker>();
            });
}
