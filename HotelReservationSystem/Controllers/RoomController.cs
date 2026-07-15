using HotelReservationSystem.Models;
using HotelReservationSystem.Repositories.Rooms;
using HotelReservationSystem.Repositories.RoomTypes;
using HotelReservationSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomRepo _repo;
        private readonly IRoomTypeRepo _roomTypeRepo;
        public RoomController (IRoomRepo repo, IRoomTypeRepo roomTypeRepo)
        {
            _repo = repo;
            _roomTypeRepo = roomTypeRepo;
        }
       
        public async Task<IActionResult> Index ()
        {
            var result = await _repo.GetAllRoomsAsync();
            if (result == null)
                return BadRequest();
            return View(result);       
        }

        public async Task<IActionResult> AddRoom()
        {
            RoomCreateVM vm = new RoomCreateVM();
            vm.roomTypes = await _roomTypeRepo.GetAllRoomTypesAsync();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoom (RoomCreateVM vm)
        {
           if (!ModelState.IsValid)
            {
                vm.roomTypes = await _roomTypeRepo.GetAllRoomTypesAsync();
                return View(vm);
            }
            //foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            //     Console.WriteLine(error.ErrorMessage);
            Room room = new Room
            {
                RoomNumber = vm.RoomNumber,
                Description = vm.Description,
                Capacity = vm.Capacity,
                PricePerNight = vm.PricePerNight,
                RoomTypeId = vm.RoomTypeId,
            };
            await _repo.AddRoomAsync(room, vm.File);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> UpdateRoom(int id)
        {
            var result = await _repo.GetRoomByIdAsync(id);
            if (result == null)
                return NotFound();
            var vm = new RoomCreateVM
            {
                Id = result.Id,
                RoomNumber = result.RoomNumber,
                Description = result.Description,
                Capacity= result.Capacity,
                PricePerNight = result.PricePerNight,
                RoomTypeId = result.RoomTypeId,
                ImagePath = result.ImagePath,
                roomTypes = await _roomTypeRepo.GetAllRoomTypesAsync()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRoom(RoomCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.roomTypes = await _roomTypeRepo.GetAllRoomTypesAsync();
                return View(vm);
            }

            var room = await _repo.GetRoomByIdAsync(vm.Id);
            if (room == null)
                return NotFound();

            room.RoomNumber = vm.RoomNumber;
            room.Description = vm.Description;
            room.Capacity = vm.Capacity;
            room.PricePerNight = vm.PricePerNight;
            room.RoomTypeId = vm.RoomTypeId;
            await _repo.UpdateRoomAsync(room.Id, room, vm.File);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            await _repo.DeleteRoomByIdAsync(id);
            return RedirectToAction("Index");
        }
    }
}
