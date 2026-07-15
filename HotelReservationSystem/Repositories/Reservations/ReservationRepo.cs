using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Enums;
using Azure.Core;

namespace HotelReservationSystem.Repositories.Reservations
{
    public class ReservationRepo : IReservationRepo
    {

        private readonly AppDbContext _context;
        public ReservationRepo (AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddReservationAsync(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
            return "Reservation Added Successfully";
        }

        public async Task<string> CancelReservationByIdAsync(int id)
        {
            var reservation = await _context.Reservations.FirstOrDefaultAsync(r  => r.Id == id);
            if (reservation != null)
            {
                reservation.Status = ReservationStatus.Cancelled;
                await _context.SaveChangesAsync();
                return "Reservation Deleted Successfully";
            }
            return "Not Found";
        }

        public async Task<List<Reservation>> GetAllReservationAsync()
        {
            return await _context.Reservations.Include(r => r.room)
                                              .ThenInclude(room => room.RoomType)
                                              .Include(r => r.user)
                                              .ToListAsync();
        }

        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            return await _context.Reservations.Include(r => r.room)
                                             .FirstOrDefaultAsync(r => r.Id==id);
        }

        public async Task<List<Reservation>> GetReservationsUserByIdAsync(string userId)
        {
            return await _context.Reservations.Include(r => r.room)
                                              .ThenInclude(room => room.RoomType)
                                              .Where(r => r.UserId == userId)
                                              .ToListAsync();
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime CheckIn, DateTime CheckOut)
        {
            return !await _context.Reservations.AnyAsync(r =>
                                                         r.RoomId == roomId &&
                                                         r.Status != ReservationStatus.Cancelled &&
                                                         r.CheckIn < CheckOut &&
                                                         r.CheckOut > CheckIn);
        }

        public async Task<string> UpdateReservationAsync(int id, int roomId, DateTime CheckIn, DateTime CheckOut)
        {
            var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);
            if (reservation == null)
                return "Not Found";
            if (CheckOut <= CheckIn)
                return "Check-out date must be after check-in date";
            bool isAvailable = !await _context.Reservations.AnyAsync(r =>
                                                                     r.Id != id &&
                                                                     r.RoomId == roomId &&
                                                                     r.Status != ReservationStatus.Cancelled &&
                                                                     r.CheckIn < CheckOut &&
                                                                     r.CheckOut > CheckIn);
            if (!isAvailable)
                return "Room is not available for the selected date";
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null)
            {
                return "Room not Found";
            }

            int nights = (CheckOut - CheckIn).Days;

            reservation.RoomId = roomId;
            reservation.CheckIn = CheckIn;
            reservation.CheckOut = CheckOut;
            reservation.TotalPrice = nights * room.PricePerNight;
            await _context.SaveChangesAsync();
            return "Reservation Updated Successfully";
        }

       
    }
}
