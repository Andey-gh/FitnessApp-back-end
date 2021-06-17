using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _emailServiceConfiguration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
             _emailServiceConfiguration = _configuration.GetSection("EmailService");
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(subject, _emailServiceConfiguration.GetValue<string>("Address")));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailServiceConfiguration.GetValue<string>("Host"), _emailServiceConfiguration.GetValue<int>("Port"), true);
                await client.AuthenticateAsync(_emailServiceConfiguration.GetValue<string>("Address"), _emailServiceConfiguration.GetValue<string>("Password"));
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
