using HotelReservationSystem.Enums;
using HotelReservationSystem.Models;
using HotelReservationSystem.Repositories.Reservations;
using HotelReservationSystem.Repositories.Rooms;
using HotelReservationSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HotelReservationSystem.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationRepo _reservationRepo;
        private readonly IRoomRepo _roomRepo;
        private readonly UserManager<IdentityUser> _userManager;
        public ReservationController(IReservationRepo reservationRepo, IRoomRepo roomRepo, UserManager<IdentityUser> userManager) 
        { 
            _reservationRepo = reservationRepo;
            _roomRepo = roomRepo;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            List<Reservation> reservations;
            if (User.IsInRole("Admin"))
                reservations = await _reservationRepo.GetAllReservationAsync();
            else
            {
                var userId = _userManager.GetUserId(User);
                reservations = await _reservationRepo.GetReservationsUserByIdAsync(userId);
            }
            return View(reservations);
        }

        [HttpGet]
        public async Task<IActionResult> AddReservation(int? roomId)
        {
            var rooms = await _roomRepo.GetAllRoomsAsync();
            var vm = new ReservationCreateVM
            {
                AvailableRooms = rooms,
                CheckIn = DateTime.Today,
                CheckOut = DateTime.Today,
            };

            if (roomId.HasValue)
                vm.RoomId = roomId.Value;

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReservation (ReservationCreateVM vm)
        {
            if (vm.CheckOut <= vm.CheckIn)
            {
                ModelState.AddModelError("", "Check-out date must be after check-in date");
                vm.AvailableRooms = await _roomRepo.GetAllRoomsAsync();
                return View(vm);
            }
            var room = await _roomRepo.GetRoomByIdAsync(vm.RoomId);
            if (room == null)
            {
                ModelState.AddModelError("", "Selected room doesn't exist");
                vm.AvailableRooms = await _roomRepo.GetAllRoomsAsync();
                return View(vm);
            }

            var isAvailable = await _reservationRepo.IsRoomAvailableAsync(vm.RoomId, vm.CheckIn, vm.CheckOut);
            if (!isAvailable)
            {
                ModelState.AddModelError("", $"Room {room.RoomNumber} is not available for the selected date");
                vm.AvailableRooms = await _roomRepo.GetAllRoomsAsync();
                return View(vm);
            }

            int nights = (vm.CheckOut - vm.CheckIn).Days;
            decimal total = nights * room.PricePerNight;
            var reservation = new Reservation
            {
                RoomId = vm.RoomId,
                CheckIn = vm.CheckIn,
                CheckOut = vm.CheckOut,
                TotalPrice = total,
                Status = ReservationStatus.Peserved,
                UserId = _userManager.GetUserId(User)
            };

            await _reservationRepo.AddReservationAsync(reservation);
            TempData["Message"] = "Your reservation booked successfully";
            return RedirectToAction("Index");
        }

        
        [HttpGet]
        public async Task<IActionResult> UpdateReservation(int id)
        {
            var reservation = await _reservationRepo.GetReservationByIdAsync(id);
            if (reservation == null)
                return NotFound();

            var vm = new ReservationCreateVM
            {
                RoomId = reservation.RoomId,
                CheckIn = reservation.CheckIn ?? DateTime.Today,
                CheckOut = reservation.CheckOut ?? DateTime.Today.AddDays(1),
                AvailableRooms = await _roomRepo.GetAllRoomsAsync()
            };

            ViewBag.ReservationId = id;
            return View(vm);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateReservation (int id, ReservationCreateVM vm)
        {
            var result = await _reservationRepo.UpdateReservationAsync(id, vm.RoomId, vm.CheckIn, vm.CheckOut);

            if (result != "Reservation Updated Successfully")
            {
                ModelState.AddModelError("", result);
                vm.AvailableRooms = await _roomRepo.GetAllRoomsAsync();
                ViewBag.ReservationId = id;
                return View(vm);
            }

            TempData["Message"] = "Reservation Updated Successfully";
            return RedirectToAction("Index");
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var result = await _reservationRepo.CancelReservationByIdAsync(id);
            TempData["Message"] = result;
            return RedirectToAction("Index");
        }
    }
}
