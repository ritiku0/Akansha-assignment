using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEmailService _emailService;

        public Worker(ILogger<Worker> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) //thakursudarshan92@gmail.com
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await _emailService.SendEmailAsync("RITIK.YADAV@wuh-group.com", "Testing Email from Ritik Don't Look So Surprised", "Automating Email");
                Console.WriteLine("Mail sent sucessfully");
                await Task.Delay(3000, stoppingToken);
            }
        }
    }
}
