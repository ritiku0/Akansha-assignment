using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System;
using System.Threading.Tasks;
using WorkerService1;
using Bytescout.Spreadsheet;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(SmtpSettings smtpSettings)
    {
        _smtpSettings = smtpSettings ?? throw new ArgumentNullException(nameof(smtpSettings));

        // Logging to verify settings
        Console.WriteLine($"Server: {_smtpSettings.Server}");
        Console.WriteLine($"Port: {_smtpSettings.Port}");
        Console.WriteLine($"UseSsl: {_smtpSettings.UseSsl}");
        Console.WriteLine($"Username: {_smtpSettings.Username}");
        Console.WriteLine($"Password: {_smtpSettings.Password}");
        Console.WriteLine($"SenderName: {_smtpSettings.SenderName}");
        Console.WriteLine($"SenderEmail: {_smtpSettings.SenderEmail}");
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        if (string.IsNullOrEmpty(_smtpSettings.SenderName) || string.IsNullOrEmpty(_smtpSettings.SenderEmail))
        {
            throw new ArgumentNullException("SenderName or SenderEmail in SmtpSettings is null or empty.");
        }
        FetchDataFromExcel();
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart("plain") { Text = body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
    public void FetchDataFromExcel()
    {
        Spreadsheet spreadsheet = new Spreadsheet();

        // Load an Excel file
        spreadsheet.LoadFromFile(@"C:\Users\Yadavri\source\repos\WorkerService1\Record.xlsx");


        // Access the first worksheet
        // Worksheet worksheet = spreadsheet.Worksheets[0];
        Worksheet worksheet = spreadsheet.Workbook.Worksheets.ByName("Sheet1");
        for (int i =0; i<1; i++)
        {
            for(int j=0; j<1; j++)
            {
                Console.WriteLine(worksheet.Cell(i, j));
            }
        }
        // If the above dont work we can also use this
        // 

        // Cleanup
        spreadsheet.Dispose();
    }
}
