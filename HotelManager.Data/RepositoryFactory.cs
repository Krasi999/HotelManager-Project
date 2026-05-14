using HotelManager.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Data
{
    public enum DatabaseType
    {
        SQLite,
        SqlServer
    }

    public static class RepositoryFactory
    {
        private static DbContextOptions<HotelDbContext>? _options;
        private static DatabaseType _currentDbType;

        public static void Initialize(DatabaseType dbType, string connectionString)
        {
            _currentDbType = dbType;

            var optionsBuilder = new DbContextOptionsBuilder<HotelDbContext>();

            if (dbType == DatabaseType.SQLite)
                optionsBuilder.UseSqlite(connectionString);
            else
                optionsBuilder.UseSqlServer(connectionString);

            _options = optionsBuilder.Options;

            // Създава таблиците ако не съществуват
            using var context = new HotelDbContext(_options);
            context.Database.EnsureCreated();
        }

        // Всеки път създава НОВ контекст — така се избягва thread проблемът
        private static HotelDbContext CreateContext()
        {
            EnsureInitialized();
            return new HotelDbContext(_options!);
        }

        public static IRepository<Room> GetRoomRepository(DatabaseType dbType)
        {
            EnsureInitialized();
            return dbType == DatabaseType.SQLite
                ? new SQLiteRepository<Room>(CreateContext())
                : new SqlServerRepository<Room>(CreateContext());
        }

        public static IRepository<Guest> GetGuestRepository(DatabaseType dbType)
        {
            EnsureInitialized();
            return dbType == DatabaseType.SQLite
                ? new SQLiteRepository<Guest>(CreateContext())
                : new SqlServerRepository<Guest>(CreateContext());
        }

        public static IRepository<Reservation> GetReservationRepository(DatabaseType dbType)
        {
            EnsureInitialized();
            return dbType == DatabaseType.SQLite
                ? new SQLiteReservationRepository(CreateContext())
                : new SqlServerReservationRepository(CreateContext());
        }

        private static void EnsureInitialized()
        {
            if (_options == null)
                throw new InvalidOperationException(
                    "RepositoryFactory не е инициализирана. Извикай Initialize() първо.");
        }
    }
}