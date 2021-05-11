using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatieService.Services
{
    public interface IRoomService
    {
        Task<ActionResult<RoomResponse>> AddAsync(RoomRequest request);
        Task<ActionResult<List<RoomResponse>>> GetAllAsync();
        Task<ActionResult<RoomResponse>> GetByIdAsync(Guid id);
        Task<ActionResult<RoomResponse>> GetByNameAsync(string name);
        Task<ActionResult<RoomResponse>> UpdateAsync(Guid id, RoomRequest request);
        Task<ActionResult> DeleteRoomAsync(Guid id);
    }
}
