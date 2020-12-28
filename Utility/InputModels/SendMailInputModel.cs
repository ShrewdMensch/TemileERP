using System;
using System.Collections.Generic;

namespace Utility.InputModels
{
    public class SendMailInputModel
    {
        public string Recipient { get; set; }
        public string PdfContent { get; set; }
        public string ExcelContent { get; set; }
        public bool AttachExcel { get; set; }
        public bool AttachPdf { get; set; }
        public string VesselName { get; set; }
        public Guid BankInstructionId { get; set; }
        public List<string> CcRecipients { get; set; }

    }
}