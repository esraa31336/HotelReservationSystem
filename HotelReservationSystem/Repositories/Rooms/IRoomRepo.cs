using HotelReservationSystem.Models;

namespace HotelReservationSystem.Repositories.Rooms
{
    public interface IRoomRepo
    {
        public Task <List<Room>> GetAllRoomsAsync();
        public Task<List<Room>> GetAvailableRoomsAsync (DateTime checkIn,  DateTime checkOut);
        public Task <Room> GetRoomByIdAsync(int id);
        public Task<string> AddRoomAsync(Room room, IFormFile file);
        public Task<string> UpdateRoomAsync(int id, Room room, IFormFile? file);
        public Task<string> DeleteRoomByIdAsync(int id);

    }
}
