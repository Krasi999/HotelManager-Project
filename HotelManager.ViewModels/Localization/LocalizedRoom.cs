using HotelManager.Models;
using HotelManager.ViewModels.Base;
using HotelManager.ViewModels.Translation;
using System.ComponentModel;

namespace HotelManager.ViewModels
{
    public class LocalizedRoom : ViewModelBase
    {
        [Browsable(false)]
        public Room Original { get; }

        private string _number = string.Empty;
        private string _type = string.Empty;
        private string _pricePerNight = string.Empty;
        private string _capacity = string.Empty;
        private string _isAvailable = string.Empty;
        private string _description = string.Empty;

        public string Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }
        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }
        public string PricePerNight
        {
            get => _pricePerNight;
            set => SetProperty(ref _pricePerNight, value);
        }
        public string Capacity
        {
            get => _capacity;
            set => SetProperty(ref _capacity, value);
        }
        public string IsAvailable
        {
            get => _isAvailable;
            set => SetProperty(ref _isAvailable, value);
        }
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public LocalizedRoom(Room room)
        {
            Original = room;
            ApplyStatic();
        }

        public void ApplyStatic()
        {
            Number = Original.Number;
            Type = Translator.RoomType(Original.Type);
            PricePerNight = Translator.Price(
                Original.PricePerNight);
            Capacity = Original.Capacity.ToString();
            IsAvailable = Translator.Bool(Original.IsAvailable);
            Description = Original.Description;
        }

        public async Task ApplyAsync()
        {
            ApplyStatic();
            if (!string.IsNullOrWhiteSpace(Original.Description))
            {
                Description = await Translator.TranslateAsync(
                    Original.Description,
                    Translator.IsBulgarian);
            }
        }
    }
}