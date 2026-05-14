using HotelManager.Models;
using HotelManager.ViewModels.Base;
using HotelManager.ViewModels.Translation;
using System.ComponentModel;

namespace HotelManager.ViewModels
{
    public class LocalizedReservation : ViewModelBase
    {
        [Browsable(false)]
        public Reservation Original { get; }

        private string _roomNumber = string.Empty;
        private string _guestName = string.Empty;
        private string _checkIn = string.Empty;
        private string _checkOut = string.Empty;
        private string _nights = string.Empty;
        private string _totalPrice = string.Empty;
        private string _status = string.Empty;

        public string RoomNumber
        {
            get => _roomNumber;
            set => SetProperty(ref _roomNumber, value);
        }
        public string GuestName
        {
            get => _guestName;
            set => SetProperty(ref _guestName, value);
        }
        public string CheckIn
        {
            get => _checkIn;
            set => SetProperty(ref _checkIn, value);
        }
        public string CheckOut
        {
            get => _checkOut;
            set => SetProperty(ref _checkOut, value);
        }
        public string Nights
        {
            get => _nights;
            set => SetProperty(ref _nights, value);
        }
        public string TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }
        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public LocalizedReservation(Reservation res)
        {
            Original = res;
            ApplyStatic();
        }

        public void ApplyStatic()
        {
            RoomNumber = Original.Room?.Number
                         ?? Original.RoomId.ToString();
            GuestName = Original.Guest != null
                ? $"{Original.Guest.FirstName}" +
                  $" {Original.Guest.LastName}"
                : Original.GuestId.ToString();
            CheckIn = Translator.Date(Original.CheckIn);
            CheckOut = Translator.Date(Original.CheckOut);
            Nights = Original.Nights.ToString();
            TotalPrice = Translator.Price(Original.TotalPrice);
            Status = Translator.Status(Original.Status);
        }

        public async Task ApplyAsync()
        {
            RoomNumber = Original.Room?.Number
                         ?? Original.RoomId.ToString();

            if (Original.Guest != null)
            {
                string fn = await Translator.TranslateAsync(
                    Original.Guest.FirstName,
                    Translator.IsBulgarian);
                string ln = await Translator.TranslateAsync(
                    Original.Guest.LastName,
                    Translator.IsBulgarian);
                GuestName = $"{fn} {ln}".Trim();
            }
            else
            {
                GuestName = Original.GuestId.ToString();
            }

            CheckIn = Translator.Date(Original.CheckIn);
            CheckOut = Translator.Date(Original.CheckOut);
            Nights = Original.Nights.ToString();
            TotalPrice = Translator.Price(Original.TotalPrice);
            Status = Translator.Status(Original.Status);
        }
    }
}