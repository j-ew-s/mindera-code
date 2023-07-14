using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model.Entities;
using Model.Repository;
using Repository.Context;

namespace Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected BlogContext Context;
        protected DbSet<T> Table;

        public BaseRepository(BlogContext context)
        {
            this.Context = context;

            this.Table = Context.Set<T>();
        }

        public async Task<T> CreateAsync(T entity)
        {
            await Table.AddAsync(entity);
            Context.SaveChanges();
            return entity;
        }

        public void Delete(T entity)
        {
            Table.Remove(entity);
            Context.SaveChanges();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Table.AsNoTracking().ToListAsync();
        }

        public virtual async Task<T> GetAsync(Guid id)
        {
            return await Table.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task UpdateAsync(T entity)
        {
            var entry = Table.Entry(entity);
            Table.Attach(entity);
            entry.State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }
    }
}

