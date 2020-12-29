using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Persistence;

namespace Utility
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _context;

        private const int count = 10;

        /***********************************************************************************************************
           ******* Universal Entity Queries*************************************************************************
           ************************************************************************************************************/
        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _context.Set<TEntity>().AddRange(entities);
        }

        public async Task<IEnumerable<TEntity>> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<TEntity> Get<TEntity>(object id) where TEntity : class
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        public async Task<TEntity> GetByPredicate<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return await _context.Set<TEntity>().Where(predicate).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll<TEntity>() where TEntity : class
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public void Remove<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> HasValues<TEntity>() where TEntity : class
        {
            return await _context.Set<TEntity>().AnyAsync();
        }

        public EntityEntry<TEntity> Attach<TEntity>(TEntity entity) where TEntity : class
        {
            return _context.Set<TEntity>().Attach(entity);
        }

        /***********************************************************************************************************
                   ******* Deduction Entity Queries*************************************************************************
                   ************************************************************************************************************/
        public async Task<float> TotalDeduction()
        {
            var deductions = await GetAll<Deduction>();

            return deductions.Sum(deduction => deduction.Percentage);
        }

        /***********************************************************************************************************
                   ******* Payroll Entity Queries*************************************************************************
                   ************************************************************************************************************/
        public async Task<int> GetNumberOfCurrentPayrolls()
        {

            return (await GetCurrentPayrolls()).Count();
        }

        public async Task<int> GetNumberOfAllPayrolls()
        {

            return (await GetAll<Payroll>()).Count();
        }

        public async Task<IEnumerable<Payroll>> GetCurrentPayrolls()
        {
            var today = DateTime.Today.ToFormalMonthAndYear();
            var payrolls = (await GetAll<Payroll>()).Where(p => p.Date.ToFormalMonthAndYear() == today);

            return payrolls;
        }

        public async Task<IEnumerable<Payroll>> GetCurrentPayrollsByVessel(string vessel)
        {
            var payrolls = (await GetCurrentPayrolls()).Where(p => p.Vessel.ToLower() == vessel.ToLower());

            return payrolls;
        }

        public async Task<Payroll> GetPersonnelPayrollByMonth(string personnelId, DateTime date)
        {
            var payroll = (await GetAll<Payroll>()).FirstOrDefault(p => p.PersonnelId == personnelId
            && p.Date.HasSameMonthAndYearWith(date));

            return payroll;
        }

        public async Task<string> GenerateNewPayrollId()
        {
            var dateAdded = DateTime.Today;
            var payrolls = await GetAll<Payroll>();
            var payrollsForToday = payrolls.Where(c => c.Date.ToFormalDate() == dateAdded.ToFormalDate());

            var payrollId = String.Format("{0}{1:D2}{2:D2}{3:D4}",
            dateAdded.Year, dateAdded.Month, dateAdded.Day, (payrollsForToday.Count() + 1));

            return payrollId;
        }

        public async Task ReApplyVariablesToCurrentPayrolls()
        {
            var currentPayrolls = await GetCurrentPayrolls();
            var deductions = await GetAll<Deduction>();
            var totalDeductions = await TotalDeduction();

            foreach (var currentPayroll in currentPayrolls)
            {
                currentPayroll.TotalDeductedPercentage = totalDeductions;
                RemoveRange(currentPayroll.DeductionDetails);

                foreach (var deduction in deductions)
                {
                    var deductionDetail = new DeductionDetail
                    {
                        DeductionName = deduction.Name,
                        DeductedPercentage = deduction.Percentage,
                        DeductedAmount = currentPayroll.GrossPay * (deduction.Percentage / 100)
                    };

                    currentPayroll.DeductionDetails.Add(deductionDetail);
                }

                currentPayroll.Vessel = currentPayroll.Personnel.Vessel;
                currentPayroll.DailyRate = currentPayroll.Personnel.DailyRate;
                currentPayroll.PersonnelDesignation = currentPayroll.Personnel.Designation;
                currentPayroll.PaymentDetail.Bank = currentPayroll.Personnel.Bank;
                currentPayroll.PaymentDetail.AccountName = currentPayroll.Personnel.AccountName;
                currentPayroll.PaymentDetail.AccountNumber = currentPayroll.Personnel.AccountNumber;
            }
        }
        public async Task ReApplyVariablesOnCurrentPayroll(Payroll currentPayroll)
        {
            var deductions = await GetAll<Deduction>();
            var totalDeductions = await TotalDeduction();

            currentPayroll.TotalDeductedPercentage = totalDeductions;
            RemoveRange(currentPayroll.DeductionDetails);

            foreach (var deduction in deductions)
            {
                var deductionDetail = new DeductionDetail
                {
                    DeductionName = deduction.Name,
                    DeductedPercentage = deduction.Percentage,
                    DeductedAmount = currentPayroll.GrossPay * (deduction.Percentage / 100)
                };

                currentPayroll.DeductionDetails.Add(deductionDetail);
            }

            currentPayroll.PaymentDetail.Bank = currentPayroll.Personnel.Bank;
            currentPayroll.PaymentDetail.AccountName = currentPayroll.Personnel.AccountName;
            currentPayroll.PaymentDetail.AccountNumber = currentPayroll.Personnel.AccountNumber;
        }

        /***********************************************************************************************************
                ******* Personnel Related Queries*************************************************************************
                ************************************************************************************************************/
        public async Task<string> GenerateNewPersonnelId(Personnel personnel)
        {
            var personnels = await GetAll<Personnel>();
            var personnelsForToday = personnels.Where(c => c.DateJoined.ToFormalDate() == DateTime.Today.ToFormalDate());
            var dateJoined = DateTime.Now;

            var personnelId = String.Format("TEM{0}{1:D2}{2:D2}{3:D4}",
            dateJoined.Year, dateJoined.Month, dateJoined.Day, (personnelsForToday.Count() + 1));

            return personnelId;
        }
        public async Task<int> GetNumberOfActivePersonnels()
        {
            var personnels = (await GetAll<Personnel>()).Where(p => p.IsActive);

            return personnels.Count();
        }
        public async Task<int> GetNumberOfInctivePersonnels()
        {
            var personnels = (await GetAll<Personnel>()).Where(p => !p.IsActive);

            return personnels.Count();
        }
        public async Task<IEnumerable<Personnel>> GetPersonnelsWithCurrentPayroll()
        {
            var currentPayrolls = (await GetCurrentPayrolls());

            var personnelsWithCurrentPayroll = currentPayrolls.Select(p => p.Personnel);

            return personnelsWithCurrentPayroll;
        }
        public async Task<IEnumerable<Personnel>> GetActivePersonnels()
        {
            var personnels = (await GetAll<Personnel>()).Where(p => p.IsActive);

            return personnels;
        }
        public async Task<IEnumerable<Personnel>> GetAllPersonnelsOrderByStatus()
        {
            var personnels = (await GetAll<Personnel>()).OrderByDescending(p => p.IsActive);

            return personnels;
        }

        /***********************************************************************************************************
               ******* AppUsers Related Queries*************************************************************************
               ************************************************************************************************************/
        public async Task<string> GenerateNewUserStaffId(AppUser user)
        {
            var users = await GetAll<AppUser>();
            var today = DateTime.Today;
            var numberOfUsersCreatedToday = users.Where(c => c.DateOfRegistration.ToShortDate() == today.ToShortDate());
            var staffId = string.Format("TEM-{0}{1:D2}{2:D2}{3:D4}",
            today.Year, today.Month, today.Day, (numberOfUsersCreatedToday.Count() + 1));

            return staffId;
        }

        /***********************************************************************************************************
               ******* EmailSentToBankLog Related Queries*************************************************************************
               ************************************************************************************************************/
        public async Task<IEnumerable<EmailSentToBankLog>> GetCurrentEmailSentToBankLogs()
        {
            var today = DateTime.Today.ToFormalMonthAndYear();
            var emailSentToBankLogs = (await GetAll<EmailSentToBankLog>()).Where(p => p.DateAdded.ToFormalMonthAndYear() == today);

            return emailSentToBankLogs;
        }

        public async Task<EmailSentToBankLog> GetCurrentMonthEmailSentToBankLog(string vessel)
        {
            var emailSentToBankLog = (await GetCurrentEmailSentToBankLogs())
                .Where(p => p.Vessel.ToLower() == vessel.ToLower()).FirstOrDefault();

            return emailSentToBankLog;
        }
        public async Task<int> GetCurrentMonthEmailSentToBankLogCount(string vessel)
        {
            var emailSentToBankLog = await GetCurrentMonthEmailSentToBankLog(vessel);

            return emailSentToBankLog == null ? 0 : emailSentToBankLog.SentCount;
        }
        public async Task<Guid?> GetCurrentMonthEmailSentToBankLogId(string vessel)
        {
            var emailSentToBankLog = (await GetCurrentEmailSentToBankLogs())
                .Where(p => p.Vessel.ToLower() == vessel.ToLower()).FirstOrDefault();

            return emailSentToBankLog?.Id;
        }
        public async Task CreateOrUpdateEmailSentToBankLog(Guid id, AppUser user, string vessel)
        {
            var emailSentToBankLog = (await Get<EmailSentToBankLog>(id));

            if (emailSentToBankLog == null)
            {
                var newEmailSentLog = new EmailSentToBankLog { AddedBy = user, ModifiedBy = user, SentCount = 1, Vessel = vessel };

                Add(newEmailSentLog);
            }

            else
            {
                emailSentToBankLog.Vessel = vessel;
                emailSentToBankLog.ModifiedBy = user;
                emailSentToBankLog.LastModified = DateTime.Now;
                emailSentToBankLog.SentCount += 1;
            }

        }
    }
}