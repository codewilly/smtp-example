using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Domain.Extensions
{
    public static class Extensions
    {
        public static string ToStringList<T>(this IEnumerable<T> source, char separator = ',')
        {
            return string.Join(separator, source);
        }

        public static void Add(this AttachmentCollection attachments, string fileName, byte[] fileBytes)
        {
            attachments.Add(new Attachment(new MemoryStream(fileBytes), fileName));
        }

        public static void AddRange(this MailAddressCollection mailAddresses, IEnumerable<string> emails)
        {
            if (emails.Any())
                mailAddresses.Add(emails.ToStringList());
        }
    }
}
