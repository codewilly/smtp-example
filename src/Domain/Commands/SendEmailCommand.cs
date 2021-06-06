using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Commands
{
    public class SendEmailCommand
    {
        public SendEmailCommand()
        {
            ToEmails = new List<string>();
            CcEmails = new List<string>();
            BccEmails = new List<string>();
            FileUrlAttachments = new List<string>();
            FileAttachments = new List<IFormFile>();
        }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public List<string> ToEmails { get; set; }

        public List<string> CcEmails { get; set; }

        public List<string> BccEmails { get; set; }

        public List<string> FileUrlAttachments { get; set; }

        public List<IFormFile> FileAttachments { get; set; }
    }
}
