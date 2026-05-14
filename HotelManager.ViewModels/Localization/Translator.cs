using HotelManager.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace HotelManager.ViewModels.Translation
{
    public static class Translator
    {
        private static readonly HttpClient _http = new()
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        private static string _baseUrl = "http://localhost:5000";
        private static string _apiKey = string.Empty;

        public static bool IsBulgarian { get; set; } = true;

        private static readonly Dictionary<string, string>
            _cache = new(StringComparer.OrdinalIgnoreCase);

        public static void Initialize(
            string baseUrl = "http://localhost:5000",
            string apiKey = "")
        {
            _baseUrl = baseUrl.TrimEnd('/');
            _apiKey = apiKey;
        }

        // ===== Основен async превод =====

        public static async Task<string> TranslateAsync(
            string text, bool toBulgarian)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;

            bool isCyrillic = text.Any(c =>
                c >= 'а' && c <= 'я' ||
                c >= 'А' && c <= 'Я');

            if (toBulgarian && isCyrillic) return text;
            if (!toBulgarian && !isCyrillic) return text;

            string cacheKey =
                $"{text}|{(toBulgarian ? "BG" : "EN")}";
            if (_cache.TryGetValue(cacheKey, out var cached))
                return cached;

            try
            {
                var payload = new Dictionary<string, string>
                {
                    { "q",      text },
                    { "source", toBulgarian ? "en" : "bg" },
                    { "target", toBulgarian ? "bg" : "en" },
                    { "format", "text" }
                };

                if (!string.IsNullOrEmpty(_apiKey))
                    payload["api_key"] = _apiKey;

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(
                    json, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync(
                    $"{_baseUrl}/translate", content);

                if (!response.IsSuccessStatusCode) return text;

                var body = await response.Content
                    .ReadAsStringAsync();
                var doc = JsonDocument.Parse(body);
                var result = doc.RootElement
                    .GetProperty("translatedText")
                    .GetString() ?? text;

                _cache[cacheKey] = result;
                return result;
            }
            catch
            {
                return text;
            }
        }

        public static async Task<bool> IsAvailableAsync()
        {
            try
            {
                var response = await _http.GetAsync(
                    $"{_baseUrl}/languages");
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        // ===== Статични преводи — без API =====

        public static string Yes =>
            IsBulgarian ? "Да" : "Yes";
        public static string No =>
            IsBulgarian ? "Не" : "No";
        public static string Currency =>
            IsBulgarian ? "евро" : "euro";

        public static string RoomType(string type) =>
           type switch
           {
               // От английски
               "Single" => IsBulgarian ? "Единична" : "Single",
               "Double" => IsBulgarian ? "Двойна" : "Double",
               "Suite" => IsBulgarian ? "Студио" : "Suite",
               "Apartment" => IsBulgarian ? "Апартамент" : "Apartment",
               // От български (ако вече е записано на БГ в базата)
               "Единична" => IsBulgarian ? "Единична" : "Single",
               "Двойна" => IsBulgarian ? "Двойна" : "Double",
               "Студио" => IsBulgarian ? "Студио" : "Suite",
               "Апартамент" => IsBulgarian ? "Апартамент" : "Apartment",
               _ => type
           };

        public static string Status(string status) =>
            status switch
            {
                //От английски
                "Confirmed" => IsBulgarian ? "Потвърдена" : "Confirmed",
                "Cancelled" => IsBulgarian ? "Отменена" : "Cancelled",
                "Completed" => IsBulgarian ? "Завършена" : "Completed",
                //От български (ако вече е записано на БГ в базата)
                "Потвърдена" => IsBulgarian ? "Потвърдена" : "Confirmed",
                "Отменена" => IsBulgarian ? "Отменена" : "Cancelled",
                "Завършена" => IsBulgarian ? "Завършена" : "Completed",
                _ => status
            };

        public static string Price(decimal price) =>
            $"{price:F2} {Currency}";

        public static string Date(DateTime dt) =>
            dt.ToString("dd.MM.yyyy");

        public static string Bool(bool value) =>
            value ? Yes : No;

        private static readonly
            Dictionary<string, (string BG, string EN)>
            _fieldNames = new()
        {
            { "Number",        ("Номер",          "Number") },
            { "Type",          ("Тип",             "Type") },
            { "PricePerNight", ("Цена на нощувка", "Price Per Night") },
            { "IsAvailable",   ("Свободна",        "Is Available") },
            { "Capacity",      ("Капацитет",       "Capacity") },
            { "Description",   ("Описание",        "Description") },
            { "FirstName",     ("Име",             "First Name") },
            { "LastName",      ("Фамилия",         "Last Name") },
            { "Email",         ("Имейл",           "Email") },
            { "Phone",         ("Телефон",         "Phone") },
            { "EGN",           ("ЕГН",             "EGN") },
            { "DateOfBirth",   ("Дата на раждане", "Date of Birth") },
            { "CheckIn",       ("Настаняване",     "Check In") },
            { "CheckOut",      ("Напускане",       "Check Out") },
            { "TotalPrice",    ("Обща цена",       "Total Price") },
            { "Status",        ("Статус",          "Status") },
            { "Nights",        ("Нощувки",         "Nights") },
            { "Id",            ("ID",              "ID") },
        };

        public static string FieldName(string propName)
        {
            if (_fieldNames.TryGetValue(propName, out var t))
                return IsBulgarian ? t.BG : t.EN;
            return System.Text.RegularExpressions.Regex
                .Replace(propName, "([A-Z])", " $1").Trim();
        }

        public static string Value(object? value, string propName)
        {
            if (value == null) return string.Empty;

            return propName switch
            {
                "IsAvailable"
                    when value is bool b => Bool(b),
                "PricePerNight" or "TotalPrice"
                    when value is decimal d => Price(d),
                "CheckIn" or "CheckOut" or "DateOfBirth"
                    when value is DateTime dt => Date(dt),
                "Type" => RoomType(value.ToString()!),
                "Status" => Status(value.ToString()!),
                "Email" => value.ToString()!,
                _ when value is bool b2 => Bool(b2),
                _ when value is decimal d2 => Price(d2),
                _ when value is DateTime dt2 => Date(dt2),
                _ => value.ToString()!
            };
        }

        public static async Task<Dictionary<string, string>>
            LocalizeRoomAsync(Room room)
        {
            string desc = string.IsNullOrWhiteSpace(room.Description)
                ? string.Empty
                : await TranslateAsync(
                    room.Description, IsBulgarian);

            return new Dictionary<string, string>
            {
                { "Number",        room.Number },
                { "Type",          RoomType(room.Type) },
                { "PricePerNight", Price(room.PricePerNight) },
                { "Capacity",      room.Capacity.ToString() },
                { "IsAvailable",   Bool(room.IsAvailable) },
                { "Description",   desc },
            };
        }

        public static async Task<Dictionary<string, string>>
            LocalizeGuestAsync(Guest guest)
        {
            string firstName = await TranslateAsync(
                guest.FirstName, IsBulgarian);
            string lastName = await TranslateAsync(
                guest.LastName, IsBulgarian);

            return new Dictionary<string, string>
            {
                { "FirstName",   firstName },
                { "LastName",    lastName },
                { "Email",       guest.Email },
                { "Phone",       guest.Phone },
                { "EGN",         guest.EGN },
                { "DateOfBirth", Date(guest.DateOfBirth) },
                { "FullName",
                  $"{firstName} {lastName}" },
            };
        }

        public static async Task<Dictionary<string, string>>
            LocalizeReservationAsync(Reservation res)
        {
            string fn = res.Guest != null
                ? await TranslateAsync(
                    res.Guest.FirstName, IsBulgarian)
                : string.Empty;
            string ln = res.Guest != null
                ? await TranslateAsync(
                    res.Guest.LastName, IsBulgarian)
                : string.Empty;

            string guestName = res.Guest != null
                ? $"{fn} {ln}".Trim()
                : res.GuestId.ToString();

            return new Dictionary<string, string>
            {
                { "Id",         res.Id.ToString() },
                { "RoomNumber", res.Room?.Number
                                ?? res.RoomId.ToString() },
                { "GuestName",  guestName },
                { "CheckIn",    Date(res.CheckIn) },
                { "CheckOut",   Date(res.CheckOut) },
                { "Nights",     res.Nights.ToString() },
                { "TotalPrice", Price(res.TotalPrice) },
                { "Status",     Status(res.Status) },
            };
        }
    }
}