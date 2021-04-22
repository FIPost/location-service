using LocatieService.Database.Contexts;
using LocatieService.Database.Converters;
using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using LocatieService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IBuildingService _buildingService;
        private readonly IDtoConverter<Room, RoomRequest, RoomResponse> _converter;
        private readonly IDtoConverter<Building, BuildingRequest, BuildingResponse> _buildingConverter;

        public RoomController(IRoomService service,
            IBuildingService buildingService,
            IDtoConverter<Room, RoomRequest, RoomResponse> converter,
            IDtoConverter<Building, BuildingRequest, BuildingResponse> buildingConverter)
        {
            _service = service;
            _buildingService = buildingService;
            _converter = converter;
            _buildingConverter = buildingConverter;
        }

        [HttpPost]
        public async Task<ActionResult<Room>> CreateRoom(RoomRequest request)
        {
            Building building = await _buildingService.GetRawByIdAsync(request.BuildingId);
            Room room = _converter.DtoToModel(request);

            // Check if building exists.
            if (building == null)
            {
                return BadRequest($"Building with id {request.BuildingId} does not exist.");
            }

            // Check for duplication.
            if (await _service.IsDuplicateAsync(room))
            {
                return Conflict($"City with name {request.Name} and buildingId {request.BuildingId} already exists.");
            }

            return await _service.AddRoomAsync(room);
        }

        [HttpGet]
        public async Task<ActionResult<List<RoomResponse>>> GetAllRooms()
        {
            return await _service.GetAllRoomsAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<RoomResponse>> GetRoomById(Guid id)
        {
            return await _service.GetRoomByIdAsync(id);
        }

        [HttpGet]
        [Route("name/{name}")]
        public async Task<ActionResult<RoomResponse>> GetRoomByName(string name)
        {
            return await _service.GetRoomByNameAsync(name);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<Room>> DeleteRoomById(Guid id)
        {
            return await _service.DeleteRoomAsync(await _service.GetRawByIdAsync(id));
        }
    }
}
