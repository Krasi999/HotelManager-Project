using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManager.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;   
        public decimal PricePerNight { get; set; }
        public bool IsAvailable { get; set; } = true;
        public int Capacity { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}