using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Entities;
using Alpha.Models.Booking;
using Alpha.Models.Role;
using Alpha.Models.Room;
using Alpha.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : Controller
    {
        private readonly AlphaDBContext _dbContext;

        public BookingController(AlphaDBContext context)
        {
            _dbContext = context;
        }

        [HttpPost("add-booking")]
        public async Task<int> AddBooking(BookingShortInfo bookingShortInfo)
        {
            if (bookingShortInfo.DateTo<bookingShortInfo.DateFrom)
            {
                throw new Exception("Неверыный промежуток времени");

            }
            var booking = new Booking()
            {
                Status = bookingShortInfo.Status,
                DateFrom = bookingShortInfo.DateFrom,
                DateTo = bookingShortInfo.DateTo,
                UserId = bookingShortInfo.UserId,
                RoomId = bookingShortInfo.RoomId
            };
            await _dbContext.AddAsync(booking);
            await _dbContext.SaveChangesAsync();
            return booking.BookingId;
        }

        [HttpGet]
        public async Task<List<BookingModel>> GetBooking()
        {
            var booking = await _dbContext.Bookings.Select(a => new BookingModel()
            {
                BookingId = a.BookingId,
                Status = a.Status,
                DateFrom = a.DateFrom,
                DateTo = a.DateTo,
                User = new UserModel()
                {
                    UserId = a.UserId,
                    Name = a.User.Name,
                    Surname = a.User.Surname,
                    Patronymic = a.User.Patronymic,
                    Login = a.User.Login,
                    Password = a.User.Password,
                    Role = new RoleModel()
                    {
                        RoleId = a.User.RoleId,
                        Name = a.User.Role.Name
                    }
                },
                Room = new RoomModel()
                {
                    RoomId = a.RoomId,
                    Name = a.Room.Name,
                    Seats = a.Room.Seats,
                    HasProjector = a.Room.HasProjector,
                    HasWhiteboard = a.Room.HasWhiteboard,
                }
            }).ToListAsync();
            return booking;
        }

        [HttpPut("update-booking")]
        public async Task UpdateBooking([FromBody] BookingModel bookingModel)
        {
            var booking = await _dbContext.Bookings.SingleOrDefaultAsync(a => a.BookingId == bookingModel.BookingId);
            bookingModel.BookingId = bookingModel.BookingId;
            bookingModel.Status = bookingModel.Status;
            bookingModel.DateFrom = bookingModel.DateFrom;
            bookingModel.DateTo = bookingModel.DateTo;

            _dbContext.Bookings.Update(booking);
            await _dbContext.SaveChangesAsync();
        }

        [HttpDelete("del-booking/{bookingid}")]
        public async Task DeleteBooking([FromRoute] int bookingid)
        {
            var booking = await _dbContext.Bookings.SingleOrDefaultAsync(a => a.BookingId == bookingid);
            _dbContext.Bookings.Remove(booking);
            await _dbContext.SaveChangesAsync();
        }
    }
}