using HotelReservationSystem.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelReservationSystem.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public string? ImagePath { get; set; }
        [NotMapped]
        public IFormFile? clientFile { get; set; }
        public RoomStatus Status { get; set; }
        public int RoomTypeId { get; set; }
        public RoomType RoomType { get; set; }
        public ICollection<Reservation> Resrvations { get; set; }

    }
}
