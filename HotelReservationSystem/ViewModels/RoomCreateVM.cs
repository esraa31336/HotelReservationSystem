using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HotelReservationSystem.ViewModels
{
    public class RoomCreateVM
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public decimal PricePerNight { get; set; }
        public int RoomTypeId { get; set; }
        [ValidateNever]
        public RoomType RoomType { get; set; }
        [ValidateNever]
        public List<RoomType> roomTypes { get; set; }
        public IFormFile? File { get; set; }
        public string? ImagePath { get; set; }
    }
}
