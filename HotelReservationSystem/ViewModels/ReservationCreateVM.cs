using HotelReservationSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.ViewModels
{
    public class ReservationCreateVM
    {
        public int RoomId { get; set; }

        
        public DateTime CheckIn { get; set; }

        
        public DateTime CheckOut { get; set; }
        public decimal TotalPrice { get; set; }
        public List<Room>? AvailableRooms { get; set; }
    }
}
