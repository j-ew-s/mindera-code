using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Model.Business
{
	public interface IBaseBusiness<T> where T : class
	{
        public Task<IEnumerable<T>> GetAllAsync();

        public Task<T> GetAsync(Guid id);

        public Task<T> CreateAsync(T entity);

        public Task<T> UpdateAsync(T entity);

        public Task DeleteAsync(Guid id);
    }
}

