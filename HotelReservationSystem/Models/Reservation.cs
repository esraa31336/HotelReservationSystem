using HotelReservationSystem.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelReservationSystem.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public decimal TotalPrice { get; set; }
        public ReservationStatus Status { get; set; }
       
        public int RoomId { get; set; }
        public Room room { get; set; }

        public string UserId { get; set; }
        public IdentityUser user { get; set; }

    }
}
