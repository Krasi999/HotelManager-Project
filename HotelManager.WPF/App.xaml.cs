using HotelManager.Data;
using HotelManager.ViewModels.Translation;
using System.Windows;

namespace HotelManager.WPF
{
    public partial class App : Application
    {
        public static DatabaseType CurrentDatabase { get; private set; }

        protected override async void OnStartup(
            StartupEventArgs e)
        {
            base.OnStartup(e);

            CurrentDatabase = DatabaseType.SqlServer;
            string connectionString =
                CurrentDatabase == DatabaseType.SQLite
                ? "Data Source=hotel.db"
                : "Server=(localdb)\\mssqllocaldb;" +
                  "Database=HotelManagerDB;" +
                  "Trusted_Connection=True;";

            RepositoryFactory.Initialize(
                CurrentDatabase, connectionString);

            Translator.Initialize("http://localhost:5000");

            bool available = await Translator.IsAvailableAsync();
            if (!available)
            {
                MessageBox.Show(
                    "LibreTranslate не е достъпен.\n" +
                    "Стартирай Docker контейнера:\n\n" +
                    "docker run -ti --rm -p 5000:5000 " +
                    "libretranslate/libretranslate " +
                    "--load-only bg,en\n\n" +
                    "Приложението ще работи само на български.",
                    "Внимание",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }
    }
}