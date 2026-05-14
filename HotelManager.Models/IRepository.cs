using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManager.Models
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id, bool isBulgarian = true);
        Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate);
    }
}