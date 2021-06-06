using Domain.Commands;
using Domain.Extensions;
using Domain.Interfaces.Services;
using Domain.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class SmtpService : ISmtpService
    {
        private readonly IRequestService _requestService;
        private readonly EmailSettings _emailSettings;

        private readonly List<Stream> _streams = new List<Stream>();

        public SmtpService(IRequestService requestService,
                           IOptions<EmailSettings> emailOptions)
        {
            _emailSettings = emailOptions?.Value;
            _requestService = requestService;
        }

        /// <summary>
        /// Send the email
        /// </summary>
        /// <param name="command">The email parameters</param>
        /// <returns>boolean that represents success or failure</returns>
        public async Task<bool> SendEmailAsync(SendEmailCommand command)
        {
            var email = new MailMessage()
            {
                From = new MailAddress(_emailSettings.Sender, _emailSettings.DisplayName),
                Subject = command.Subject,
                Body = command.Body,
                IsBodyHtml = true
            };

            email.To.AddRange(command.ToEmails);
            email.CC.AddRange(command.CcEmails);
            email.Bcc.AddRange(command.BccEmails);

            await AddFileAttachments(command.FileAttachments, email);
            await AddFileUrlAttachments(command.FileUrlAttachments, email);
            await SendSmtpEmail(email);

            return true;
        }

        /// <summary>
        /// Opens a SMTP connection and send the email
        /// </summary>
        private async Task SendSmtpEmail(MailMessage email)
        {
            var smtp = new SmtpClient(_emailSettings.Host)
            {
                Port = _emailSettings.Port,
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                EnableSsl = _emailSettings.EnableSSL
            };

            await smtp.SendMailAsync(email);

            smtp.Dispose();

            await DisposeStreams();
        }

        /// <summary>
        /// Attaches formFiles to email
        /// </summary>
        private async Task AddFileAttachments(IEnumerable<IFormFile> attachments, MailMessage email)
        {
            if (!attachments.Any())
                return;

            foreach (IFormFile file in attachments)
            {
                if (file.Length > 0)
                {
                    byte[] fileBytes;

                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);

                        fileBytes = memoryStream.ToArray();
                    }

                    email.Attachments.Add(file.FileName, fileBytes);
                }
            }
        }

        /// <summary>
        /// Downloads a file by a url and then attaches it to mail
        /// </summary>
        private async Task AddFileUrlAttachments(IEnumerable<string> urls, MailMessage email)
        {
            if (!urls.Any())
                return;

            foreach (var url in urls)
            {
                (Stream stream, string fileName) = await _requestService.DownloadAsStream(url);

                email.Attachments.Add(new Attachment(stream, fileName));

                _streams.Add(stream);
            }
        }

        /// <summary>
        /// Dispose streams
        /// </summary>
        /// <returns></returns>
        private async Task DisposeStreams()
        {
            foreach (var stream in _streams)
                await stream.DisposeAsync();
        }
    }
}
