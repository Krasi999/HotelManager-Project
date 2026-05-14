using HotelManager.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Data
{
    public abstract class BaseRepository<T> : IRepository<T>, IDisposable
        where T : class
    {
        protected readonly HotelDbContext _context;
        protected readonly DbSet<T> _dbSet;

        protected BaseRepository(HotelDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(int id,
            bool isBulgarian = true)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return;

            if (entity is Room room)
            {
                bool hasReservations = await _context.Reservations
                    .AnyAsync(r => r.RoomId == room.Id);

                if (hasReservations)
                {
                    throw new InvalidOperationException(
                        isBulgarian
                        ? $"Стая {room.Number} не може да бъде изтрита," +
                          $" защото има активни резервации."
                        : $"Room {room.Number} cannot be deleted" +
                          $" because it has active reservations.");
                }
            }

            if (entity is Guest guest)
            {
                bool hasReservations = await _context.Reservations
                    .AnyAsync(r => r.GuestId == guest.Id);

                if (hasReservations)
                {
                    throw new InvalidOperationException(
                        isBulgarian
                        ? $"Гост {guest.FirstName} {guest.LastName}" +
                          $" не може да бъде изтрит," +
                          $" защото има активни резервации."
                        : $"Guest {guest.FirstName} {guest.LastName}" +
                          $" cannot be deleted" +
                          $" because they have active reservations.");
                }
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(
            Func<T, bool> predicate)
        {
            return await Task.FromResult(
                _dbSet.AsNoTracking()
                      .Where(predicate)
                      .ToList());
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}