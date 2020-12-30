using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Pages;
using Domain;
using Utility;
using Utility.DTOs;
using Utility.InputModels;
using Microsoft.EntityFrameworkCore;
using static Utility.UtilityClasses;
using System.Linq;
using SendGrid.Helpers.Mail;
using Utility.Notifications;

namespace Web.Pages.Accounting
{
    public class BankInstructionsModel : BasePageModel
    {
        private readonly ILogger<BankInstructionsModel> _logger;
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;
        private readonly ISend _send;

        public BankInstructionsModel(ILogger<BankInstructionsModel> logger, IRepository repository, IMapper mapper,
            IUserAccessor userAccessor, ISend send)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _userAccessor = userAccessor;
            _send = send;
        }



        [BindProperty]
        public IEnumerable<InstructionToBankListDto> BankInstructions { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var payrolls = await _repository.GetCurrentPayrolls();

            var bankInstructions = payrolls.GroupBy(p => p.Vessel, (vessel, payroll) => new InstructionToBankListDto
            {
                Id = (_repository.GetCurrentMonthEmailSentToBankLogId(vessel).Result.GetValueOrDefault()),
                Title = String.Format("Instructions To Bank For {0} Vessel", vessel),
                Vessel = vessel,
                Date = payroll.FirstOrDefault()?.Date.ToFormalMonthAndYear(),
                PersonnelCount = payroll.Count(),
                GrandTotal = payroll.Sum(p => p.NetPay).ToCurrency(),
                SentCount = (_repository.GetCurrentMonthEmailSentToBankLogCount(vessel).Result)
            });

            BankInstructions = bankInstructions.DistinctByVessel();

            return Page();
        }

        public async Task<IActionResult> OnPostSendMailAsync(SendMailInputModel sendMailInput)
        {
            var message = new EmailMessage();
            var monthYear = DateTime.Now.ToFormalMonthAndYear();
            var subject = "Monthly Payroll For " + monthYear;

            message.Subject = subject;

            subject = subject.Replace(", ", "_");

            PrepareMailMessage(sendMailInput, message, monthYear, subject);

            if (await _send.Mail(message))
            {
                await _repository.CreateOrUpdateEmailSentToBankLog(sendMailInput.BankInstructionId,
                    _userAccessor.GetCurrentUser(), sendMailInput.VesselName);

                if (await _repository.SaveAll())
                {
                    SetNotificationMessageAndIcon("Email sent to Bank successfully", MessageType.Success);

                }

                else
                {
                    SetNotificationMessageAndIcon("Email could not be sent to Bank", MessageType.Error);
                }
            }

            else
            {
                SetNotificationMessageAndIcon("Email could not be sent to Bank", MessageType.Error);
            }

            return RedirectToPage();
        }

        private static void PrepareMailMessage(SendMailInputModel sendMailInput, EmailMessage message, string monthYear, string subject)
        {
            AddAttachments(sendMailInput, message, subject);

            AddRecipients(sendMailInput, message);

            AddMessageBody(message, monthYear);
        }

        private static void AddMessageBody(EmailMessage message, string monthYear)
        {
            message.HtmlContent = "<p>Dear Banker, <br><br> Find attached Temile and Sons Employees' Pay Slip for " + monthYear +
                            "<br><br>Best Regards, <br>Temile and Sons Limited</p>";

            message.PlainTextContent = "FYA";
        }

        private static void AddRecipients(SendMailInputModel sendMailInput, EmailMessage message)
        {
            message.ToRecipients.Add(new EmailAddress(sendMailInput.Recipient));


            if (sendMailInput.CcRecipients != null)
            {
                foreach (var ccRecipient in sendMailInput.CcRecipients)
                {
                    message.CcRecipients.Add(new EmailAddress(ccRecipient));
                }
            }
        }

        private static void AddAttachments(SendMailInputModel sendMailInput, EmailMessage message, string subject)
        {
            if (sendMailInput.AttachExcel)
            {
                message.Attachments.Add(new MailAttachment()
                {
                    Content = sendMailInput.ExcelContent,
                    FileName = subject + ".xlsx"
                });
            }

            if (sendMailInput.AttachPdf)
            {
                message.Attachments.Add(new MailAttachment()
                {
                    Content = sendMailInput.PdfContent,
                    FileName = subject + ".pdf"
                });

            }
        }
    }
}