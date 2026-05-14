using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManager.Models
{
    public class Guest
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string EGN { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}