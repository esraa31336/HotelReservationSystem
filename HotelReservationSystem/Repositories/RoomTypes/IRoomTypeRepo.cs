using HotelReservationSystem.Models;

namespace HotelReservationSystem.Repositories.RoomTypes
{
    public interface IRoomTypeRepo
    {
        public Task<List<RoomType>> GetAllRoomTypesAsync();
        public Task<RoomType> GetRoomTypeByIdAsync(int id);
        public Task<string> AddRoomTypeAsync(RoomType roomType);
        public Task<string> UpdateRoomTypeAsync (int id, RoomType roomType);
        public Task<string> DeleteRoomTypeByIdAsync(int id);
        
    }
}
