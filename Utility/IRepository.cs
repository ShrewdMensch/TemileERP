using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Utility
{
    public interface IRepository
    {
        Task<TEntity> Get<TEntity>(object id) where TEntity : class;
        Task<TEntity> GetByPredicate<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        Task<IEnumerable<TEntity>> GetAll<TEntity>() where TEntity : class;
        Task<IEnumerable<TEntity>> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        void Add<TEntity>(TEntity entity) where TEntity : class;
        void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        EntityEntry<TEntity> Attach<TEntity>(TEntity entity) where TEntity : class;


        void Remove<TEntity>(TEntity entity) where TEntity : class;
        void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        Task<bool> HasValues<TEntity>() where TEntity : class;
        Task<bool> SaveAll();


        //Deduction Entity Related
        Task<float> TotalDeduction();


        //Personnel Entity Related
        Task<string> GenerateNewPersonnelId(Personnel personnel);
        Task<int> GetNumberOfActivePersonnels();
        Task<int> GetNumberOfInctivePersonnels();
        Task<IEnumerable<Personnel>> GetPersonnelsWithCurrentPayroll();
        Task<IEnumerable<Personnel>> GetActivePersonnels();
        Task<IEnumerable<Personnel>> GetAllPersonnelsOrderByStatus();

        //AppUser Related
        Task<string> GenerateNewUserStaffId(AppUser user);


        //Payroll Entity Related
        Task<int> GetNumberOfCurrentPayrolls();
        Task<IEnumerable<Payroll>> GetCurrentPayrolls();
        Task<IEnumerable<Payroll>> GetCurrentPayrollsByVessel(string vessel);
        Task<Payroll> GetPersonnelPayrollByMonth(string personnelId, DateTime date);
        Task<int> GetNumberOfAllPayrolls();
        Task<string> GenerateNewPayrollId();
        Task ReApplyVariablesToCurrentPayrolls();
        Task ReApplyVariablesOnCurrentPayroll(Payroll currentPayroll);

        //EmailSentToBankLog Entity Related
        Task<EmailSentToBankLog> GetCurrentMonthEmailSentToBankLog(string vessel);
        Task<int> GetCurrentMonthEmailSentToBankLogCount(string vessel);
        Task<IEnumerable<EmailSentToBankLog>> GetCurrentEmailSentToBankLogs();
        Task<Guid?> GetCurrentMonthEmailSentToBankLogId(string vessel);
        Task CreateOrUpdateEmailSentToBankLog(Guid id, AppUser user, string vessel);
    }
}