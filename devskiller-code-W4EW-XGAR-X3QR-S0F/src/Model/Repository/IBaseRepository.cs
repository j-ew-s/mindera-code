using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model.Entities;

namespace Model.Repository
{
	public interface IBaseRepository<T> where T: BaseEntity
	{
        public Task<IEnumerable<T>> GetAllAsync();

        public Task<T> GetAsync(Guid id);

        public Task<T> CreateAsync(T post);

        public Task UpdateAsync(T post);

        public void Delete(T entity);
    }
}
