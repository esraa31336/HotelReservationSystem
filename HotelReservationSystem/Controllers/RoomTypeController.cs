using HotelReservationSystem.Models;
using HotelReservationSystem.Repositories.RoomTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoomTypeController : Controller
    {
        private readonly IRoomTypeRepo _repo;
        public RoomTypeController (IRoomTypeRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roomType = await _repo.GetAllRoomTypesAsync();
            if (roomType == null) 
                return BadRequest();
            return View(roomType);
        }

        public IActionResult AddRoomType()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoomType(RoomType roomType)
        {
            if(ModelState.IsValid)
            {
                await _repo.AddRoomTypeAsync(roomType);
                return RedirectToAction("Index");
            }
            return View(roomType);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateRoomType(int id)
        {
            var roomT = await _repo.GetRoomTypeByIdAsync(id);
            if (roomT == null)
                return BadRequest();
            return View(roomT);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRoomType(RoomType roomType)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            await _repo.UpdateRoomTypeAsync(roomType.Id, roomType);
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRoomType(int id)
        {
            await _repo.DeleteRoomTypeByIdAsync(id);
            return RedirectToAction("Index");
        }
    }
}
