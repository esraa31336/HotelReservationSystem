using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Enums;

namespace HotelReservationSystem.Repositories.Rooms
{
    public class RoomRepo : IRoomRepo
    {
        private readonly AppDbContext _context;
        public RoomRepo (AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddRoomAsync(Room room, IFormFile file)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName);
            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
            room.ImagePath = fileName;
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
            return "Room was added Successfully";
        }

        public async Task<string> DeleteRoomByIdAsync(int id)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
                return "Not Found";
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return "Room was deleted successfully";
        }

        public async Task<List<Room>> GetAllRoomsAsync()
        {
            return await _context.Rooms.Include(r => r.RoomType)
                                       .ToListAsync();
        }

        public async Task<List<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut)
        {
            var bookedRoomId = await _context.Reservations
                                             .Where(r => r.Status != ReservationStatus.Cancelled && 
                                                          r.CheckIn < r.CheckOut && r.CheckOut > r.CheckIn)
                                             .Select(r => r.RoomId)
                                             .ToListAsync();
            return await _context.Rooms.Include(r => r.RoomType)
                                       .Where(r => !bookedRoomId.Contains(r.Id))
                                       .ToListAsync();
        }

        public async Task<Room> GetRoomByIdAsync(int id)
        {
            var room = await _context.Rooms.Include(r => r.RoomType)
                                           .FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
                return null;
            return room;
        }

        public async Task<string> UpdateRoomAsync(int id, Room room, IFormFile? file)
        {
            var existing = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
            if (existing == null)
                return "Not Found";

            existing.RoomNumber = room.RoomNumber;
            existing.Capacity = room.Capacity;
            existing.PricePerNight = room.PricePerNight;
            existing.Description = room.Description;
            existing.Status = room.Status;
            existing.RoomTypeId = room.RoomTypeId;

            if (file != null)
            {
                
                if (!string.IsNullOrEmpty(existing.ImagePath))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", existing.ImagePath);
                    if (File.Exists(oldPath))
                        File.Delete(oldPath);
                }

                
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var newPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName);

                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                existing.ImagePath = fileName;
            }

            await _context.SaveChangesAsync();
            return "Room was Updated successfully";
        }
    }
}
