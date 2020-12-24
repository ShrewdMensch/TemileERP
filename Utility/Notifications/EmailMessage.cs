using SendGrid.Helpers.Mail;
using System.Collections.Generic;

namespace Utility.Notifications
{
    public class EmailMessage
    {
        public EmailMessage()
        {
            ToRecipients = new List<EmailAddress>();
            CcRecipients = new List<EmailAddress>();
            BccRecipients = new List<EmailAddress>();
            Attachments = new List<MailAttachment>();
        }

        public EmailAddress ReplyTo { get; set; }
        public string Subject { get; set; }
        public string PlainTextContent { get; set; }
        public string HtmlContent { get; set; }
        public List<EmailAddress> ToRecipients { get; set; }
        public List<EmailAddress> CcRecipients { get; set; }
        public List<EmailAddress> BccRecipients { get; set; }
        public List<MailAttachment> Attachments { get; set; }
    }
}
