using HotelManager.Data;

namespace HotelManager.WinForms
{
    internal static class Program
    {
        public static DatabaseType CurrentDatabase { get; private set; }

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            CurrentDatabase = DatabaseType.SQLite;

            string connectionString = CurrentDatabase == DatabaseType.SQLite
                ? "Data Source=hotel.db"
                : "Server=(localdb)\\mssqllocaldb;" +
                  "Database=HotelManagerDB;Trusted_Connection=True;";

            RepositoryFactory.Initialize(CurrentDatabase, connectionString);

            Application.Run(new MainForm());
        }
    }
}

