using HotelManager.Models;
using HotelManager.ViewModels.Base;
using HotelManager.ViewModels.Translation;
using System.ComponentModel;

namespace HotelManager.ViewModels
{
    public class LocalizedGuest : ViewModelBase
    {
        [Browsable(false)]
        public Guest Original { get; }

        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _email = string.Empty;
        private string _phone = string.Empty;
        private string _egn = string.Empty;
        private string _dateOfBirth = string.Empty;

        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }
        public string EGN
        {
            get => _egn;
            set => SetProperty(ref _egn, value);
        }
        public string DateOfBirth
        {
            get => _dateOfBirth;
            set => SetProperty(ref _dateOfBirth, value);
        }

        public LocalizedGuest(Guest guest)
        {
            Original = guest;
            ApplyStatic();
        }

        public void ApplyStatic()
        {
            FirstName = Original.FirstName;
            LastName = Original.LastName;
            Email = Original.Email;
            Phone = Original.Phone;
            EGN = Original.EGN;
            DateOfBirth = Translator.Date(Original.DateOfBirth);
        }

        public async Task ApplyAsync()
        {
            FirstName = await Translator.TranslateAsync(
                Original.FirstName, Translator.IsBulgarian);
            LastName = await Translator.TranslateAsync(
                Original.LastName, Translator.IsBulgarian);
            Email = Original.Email;
            Phone = Original.Phone;
            EGN = Original.EGN;
            DateOfBirth = Translator.Date(Original.DateOfBirth);
        }
    }
}