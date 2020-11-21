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
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        private const int count = 10;

        /***********************************************************************************************************
           ******* Universal Entity Queries*************************************************************************
           ************************************************************************************************************/
        public Repository(ApplicationDbContext context, IUserAccessor userAccessor, IMapper mapper)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mapper = mapper;
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

        public async Task<string> GenerateNewPayrollId()
        {
            var dateAdded = DateTime.Today;
            var payrolls = await GetAll<Payroll>();
            var payrollsForToday = payrolls.Where(c => c.Date.ToFormalDate() == dateAdded.ToFormalDate());

            var payrollId = String.Format("{0}{1:D2}{2:D2}{3:D4}",
            dateAdded.Year, dateAdded.Month, dateAdded.Day, (payrollsForToday.Count() + 1));

            return payrollId;
        }

        /***********************************************************************************************************
                ******* Personnel Related Queries*************************************************************************
                ************************************************************************************************************/
        public async Task<string> GenerateNewPersonnelId(Personnel personnel)
        {
            var personnels = await GetAll<Personnel>();
            var personnelsForToday = personnels.Where(c => c.DateJoined.ToFormalDate() == DateTime.Today.ToFormalDate());
            var dateJoined = personnel.DateJoined;

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
    }
}