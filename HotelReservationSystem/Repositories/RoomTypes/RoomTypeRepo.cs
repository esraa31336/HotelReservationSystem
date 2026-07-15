using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Repositories.RoomTypes
{
    public class RoomTypeRepo : IRoomTypeRepo
    {
        private readonly AppDbContext _context;
        public RoomTypeRepo (AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoomType>> GetAllRoomTypesAsync()
        {
            return await _context.RoomTypes.ToListAsync();
        }

        public async Task<RoomType> GetRoomTypeByIdAsync(int id)
        {
            var roomType = await _context.RoomTypes.FirstOrDefaultAsync(r => r.Id == id);
            if (roomType == null)
                return null;
            return roomType;
        }

        public async Task<string> AddRoomTypeAsync(RoomType roomType)
        {
            await _context.RoomTypes.AddAsync(roomType);
            await _context.SaveChangesAsync();
            return "Room Type added successfuly";
        }

        public async Task<string> DeleteRoomTypeByIdAsync(int id)
        {
            var roomType = await _context.RoomTypes.FirstOrDefaultAsync(r => r.Id == id);
            if (roomType != null)
            {
                _context.RoomTypes.Remove(roomType);
                await _context.SaveChangesAsync();
                return "Room Type deleted Successfully";
            }
            else
                return "Room Type Not Found";
        }

        public async Task<string> UpdateRoomTypeAsync(int id, RoomType roomType)
        {
            var existingRoomType = await _context.RoomTypes.FirstOrDefaultAsync(r => r.Id == id);
            if (existingRoomType != null)
            {
                existingRoomType.Name = roomType.Name;
                existingRoomType.Description = roomType.Description;
                await _context.SaveChangesAsync();
                return "Room Type Updated Successfully";
            }
            else
                return "Not Found";
        }
    }
}
