using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DearlerPlatform.Domain;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;

namespace DearlerPlatform.Core.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DealerPlatformContext _context;

        public Repository(DealerPlatformContext context)
        {
            this._context = context;
        }
        //查询表所有数据
        public List<TEntity> GetList()
        {
            //  Set<TEntity>()应为其返回一个集的实体的类型
            var dbSet = _context.Set<TEntity>();
            return dbSet.ToList();
        }
        public IQueryable<TEntity> GetQueryable(){
            var dbSet =_context.Set<TEntity>();
            return dbSet;
        }
        public List<TEntity> GetList(Func<TEntity, bool> predicate)
        {
            var dbSet = _context.Set<TEntity>();
            return dbSet.Where(predicate).ToList();
        }

        public async Task<List<TEntity>> GetListAsync()
        {
            var dbSet = _context.Set<TEntity>();
            return await dbSet.ToListAsync();
        }
        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var dbSet = _context.Set<TEntity>();
            return await dbSet.Where(predicate).ToListAsync();
        }
        //返回实体
        public TEntity Get(Func<TEntity, bool> predicate)
        {
            var dbSet = _context.Set<TEntity>();
            var res = dbSet.FirstOrDefault(predicate);
            return res;
        }
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var dbSet = _context.Set<TEntity>();
            var res = await dbSet.FirstOrDefaultAsync(predicate);
            return res;

        }
        public TEntity Insert(TEntity entity)
        {
            var dbSet = _context.Set<TEntity>();
            var res = dbSet.Add(entity).Entity;
            _context.SaveChanges();
            return res;
        }
        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            var dbSet = _context.Set<TEntity>();
            var res = (await dbSet.AddAsync(entity)).Entity;
            await _context.SaveChangesAsync();
            return res;
        }
        public TEntity Delete(TEntity entity)
        {
            var dbSet = _context.Set<TEntity>();
            var res = dbSet.Remove(entity).Entity;
            _context.SaveChanges();
            return res;
        }
        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            var dbSet = _context.Set<TEntity>();
            var res = dbSet.Remove(entity).Entity;
            await _context.SaveChangesAsync();
            return res;
        }
        public TEntity Update(TEntity entity)
        {
            var dbSet = _context.Set<TEntity>();
            var res = dbSet.Update(entity).Entity;
            _context.SaveChanges();
            return res;
        }
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var dbSet = _context.Set<TEntity>();
            var res = dbSet.Update(entity).Entity;
            await _context.SaveChangesAsync();
            return res;
        }
    }
}