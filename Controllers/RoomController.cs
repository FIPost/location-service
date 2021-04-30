using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using LocatieService.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatieService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : Controller
    {
        private readonly IRoomService _service;

        public RoomController(IRoomService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<RoomResponse>> AddRoom(RoomRequest request)
        {
            return await _service.AddAsync(request);
        }

        [HttpGet]
        public async Task<ActionResult<List<RoomResponse>>> GetAllRooms()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<RoomResponse>> GetRoomById(Guid id)
        {
            return await _service.GetByIdAsync(id);
        }

        [HttpGet]
        [Route("name/{name}")]
        public async Task<ActionResult<RoomResponse>> GetRoomByName(string name)
        {
            return await _service.GetByNameAsync(name);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<RoomResponse>> UpdateRoom(Guid id, RoomRequest request)
        {
            return await _service.UpdateAsync(id, request);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteRoomById(Guid id)
        {
            await _service.DeleteRoomAsync(id);

            return NoContent();
        }
    }
}
