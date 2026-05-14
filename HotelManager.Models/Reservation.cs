using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManager.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int GuestId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Confirmed";
        public Room? Room { get; set; }
        public Guest? Guest { get; set; }
        public int Nights => (CheckOut - CheckIn).Days;
    }
}
