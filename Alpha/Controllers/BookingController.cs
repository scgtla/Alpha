using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.DataBase;
using Alpha.DataBase.Entities;
using Alpha.Models.Booking;
using Alpha.Models.Role;
using Alpha.Models.Room;
using Alpha.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Controllers
{
    [Authorize]
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
        public async Task<int> AddBooking(BookingCreate bookingCreate)
        {
            if (bookingCreate.DateTo < bookingCreate.DateFrom)
            {
                throw new Exception("Неверыный промежуток времени");
            }

            var isValid = _dbContext.Bookings.Any(a =>
                a.RoomId == bookingCreate.RoomId &&
                a.DateFrom == bookingCreate.DateFrom &&
                a.DateTo == bookingCreate.DateTo &&
                a.Status == true);

            if (isValid)
            {
                throw new Exception("Данная комната на данное время уже забронирована");
            }

            var booking = new Booking()
            {
                DateFrom = bookingCreate.DateFrom,
                DateTo = bookingCreate.DateTo,
                UserId = bookingCreate.UserId,
                RoomId = bookingCreate.RoomId
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
        public async Task UpdateBooking([FromBody] BookingCreate bookingCreate)
        {
            if (bookingCreate.DateTo < bookingCreate.DateFrom)
            {
                throw new Exception("Неверыный промежуток времени");
            }

            var isValid = _dbContext.Bookings.Any(a =>
                a.RoomId == bookingCreate.RoomId &&
                a.DateFrom == bookingCreate.DateFrom &&
                a.DateTo == bookingCreate.DateTo &&
                a.Status == true);

            if (isValid)
            {
                throw new Exception("Данная комната на данное время уже забронирована");
            }

            var booking = await _dbContext.Bookings.SingleOrDefaultAsync(a => a.BookingId == bookingCreate.BookingId);
            booking.DateFrom = bookingCreate.DateFrom;
            booking.DateTo = bookingCreate.DateTo;


            _dbContext.Bookings.Update(booking);
            await _dbContext.SaveChangesAsync();
        }

        [HttpDelete("del-booking/{bookingid}")]
        public async Task DeleteBooking([FromRoute] int bookingid)
        {
            Booking booking = await _dbContext.Bookings.SingleOrDefaultAsync(a => a.BookingId == bookingid);
            _dbContext.Bookings.Remove(booking);
            await _dbContext.SaveChangesAsync();
        }

        [Authorize(Roles = "OfficeManager")]
        [HttpPut("update-status")]
        public async Task UpdateStatus([FromRoute] BookingStatus bookingStatus)
        {
            var booking = await _dbContext.Bookings.SingleOrDefaultAsync(a => a.BookingId == bookingStatus.BookingId);
            booking.Status = bookingStatus.Status;
            _dbContext.Bookings.Update(booking);
            await _dbContext.SaveChangesAsync();
        }
    }
}