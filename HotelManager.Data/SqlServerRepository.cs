using HotelManager.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Data
{
    public class SqlServerRepository<T> : BaseRepository<T> where T : class
    {
        public SqlServerRepository(HotelDbContext context) : base(context)
        {
        }
    }

    public class SqlServerReservationRepository
    : SqlServerRepository<Reservation>
    {
        public SqlServerReservationRepository(HotelDbContext context)
            : base(context) { }

        public override async Task AddAsync(Reservation entity)
        {
            await base.AddAsync(entity);

            var room = await _context.Rooms.FindAsync(entity.RoomId);
            if (room != null)
            {
                room.IsAvailable = false;
                await _context.SaveChangesAsync();
            }
        }

        public override async Task DeleteAsync(int id,
            bool isBulgarian = true)
        {
            var reservation = await GetByIdAsync(id);
            if (reservation == null) return;

            var room = await _context.Rooms
                .FindAsync(reservation.RoomId);

            await base.DeleteAsync(id, isBulgarian);

            if (room != null)
            {
                bool hasOtherReservations = await _context.Reservations
                    .AnyAsync(r => r.RoomId == room.Id);

                if (!hasOtherReservations)
                {
                    room.IsAvailable = true;
                    await _context.SaveChangesAsync();
                }
            }
        }

        public override async Task<IEnumerable<Reservation>>
            GetAllAsync()
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .ToListAsync();
        }

        public override async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}