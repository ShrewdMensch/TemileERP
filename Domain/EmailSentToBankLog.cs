using Domain.Utility;
using System;

namespace Domain
{
    public class EmailSentToBankLog : History
    {
        public EmailSentToBankLog()
        {
            DateAdded = DateTime.Now;
            LastModified = DateTime.Now;
        }
        public Guid Id { get; set; }
        public string Vessel { get; set; }
        public int SentCount { get; set; }
    }
}