using HotelReservationSystem.Models;
using HotelReservationSystem.Enums;

namespace HotelReservationSystem.Repositories.Reservations
{
    public interface IReservationRepo
    {
        public Task<List<Reservation>> GetAllReservationAsync();
        public Task<Reservation> GetReservationByIdAsync(int id);
        public Task<List<Reservation>> GetReservationsUserByIdAsync(string userId);
        public Task<bool> IsRoomAvailableAsync(int roomId, DateTime CheckIn, DateTime CheckOut);
        public Task<string> AddReservationAsync(Reservation reservation);
        public Task<string> UpdateReservationAsync (int id, int roomId, DateTime CheckIn, DateTime CheckOut);
        public Task<string> CancelReservationByIdAsync(int id);
    }
}
