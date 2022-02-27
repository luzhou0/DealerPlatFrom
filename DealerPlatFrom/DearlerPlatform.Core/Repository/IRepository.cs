using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DearlerPlatform.Core.Repository
{
    public interface IRepository<TEntity>
    {
        TEntity Delete(TEntity entity);
        Task<TEntity> DeleteAsync(TEntity entity);
        TEntity Get(Func<TEntity, bool> predicate);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
        List<TEntity> GetList();
        List<TEntity> GetList(Func<TEntity, bool> predicate);
        Task<List<TEntity>> GetListAsync();
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity Insert(TEntity entity);
        Task<TEntity> InsertAsync(TEntity entity);
        TEntity Update(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        IQueryable<TEntity> GetQueryable();
    }
}