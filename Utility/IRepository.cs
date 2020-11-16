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
        Task<int> GetNumberOfActivePersonnels();
        Task<int> GetNumberOfInctivePersonnels();


        //Payroll Entity Related
        Task<int> GetNumberOfCurrentPayrolls();
        Task<int> GetNumberOfAllPayrolls();
        Task<IEnumerable<Payroll>> GetCurrentPayrolls();

    }
}