using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.DataBase;
using Alpha.DataBase.Entities;
using Alpha.Models.Room;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : Controller
    {
        private readonly AlphaDBContext _dbContext;

        public RoomController(AlphaDBContext context)
        {
            _dbContext = context;
        }

        [Authorize(Roles = "OfficeManager")]
        [HttpPost("add-room")]
        public async Task<int> AddRoom(RoomShortInfo roomModel)
        {
            var room = new Room()
            {
                Name = roomModel.Name,
                Seats = roomModel.Seats,
                HasProjector = roomModel.HasProjector,
                HasWhiteboard = roomModel.HasWhiteboard
            };
            await _dbContext.AddAsync(room);
            await _dbContext.SaveChangesAsync();
            return room.RoomId;
        }

        [Authorize]
        [HttpGet]
        public async Task<List<RoomModel>> GetRooms()
        {
            var rooms = await _dbContext.Rooms.Select(a => new RoomModel()
            {
                RoomId = a.RoomId,
                Name = a.Name,
                Seats = a.Seats,
                HasProjector = a.HasProjector,
                HasWhiteboard = a.HasWhiteboard,
            }).ToListAsync();
            return rooms;
        }

        [Authorize(Roles = "OfficeManager")]
        [HttpPut("update-room")]
        public async Task UpdateRoom([FromBody] RoomModel roomModel)
        {
            var room = await _dbContext.Rooms.SingleOrDefaultAsync(a => a.RoomId == roomModel.RoomId);
            room.Name = roomModel.Name;
            room.Seats = roomModel.Seats;
            room.HasProjector = roomModel.HasProjector;
            room.HasWhiteboard = roomModel.HasWhiteboard;

            _dbContext.Rooms.Update(room);
            await _dbContext.SaveChangesAsync();
        }

        [Authorize(Roles = "OfficeManager")]
        [HttpDelete("del-room/{roomid}")]
        public async Task DeleteRoom([FromRoute] int roomid)
        {
            var room = await _dbContext.Rooms.SingleOrDefaultAsync(a => a.RoomId == roomid);
            _dbContext.Rooms.Remove(room);
            await _dbContext.SaveChangesAsync();
        }
    }
}