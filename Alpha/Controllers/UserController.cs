using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Entities;
using Alpha.Models.Role;
using Alpha.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly AlphaDBContext _dbContext;

        public UserController(AlphaDBContext context)
        {
            _dbContext = context;
        }

        [HttpPost("add-user")]
        public async Task<int> AddUser(UserShortInfo shortInfo)
        {
            if (_dbContext.Users.Any(a => a.Login == shortInfo.Login))
            {
                throw new Exception($"Ошибка: Данный логин уже существует {shortInfo.Login}");
            }

            var user = new User()
            {
                Name = shortInfo.Name,
                Surname = shortInfo.Surname,
                Patronymic = shortInfo.Patronymic,
                Login = shortInfo.Login,
                Password = shortInfo.Password,
                RoleId = shortInfo.RoleId
            };
            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user.UserId;
        }

        [HttpGet]
        public async Task<List<UserModel>> GetUsers()
        {
            var users = await _dbContext.Users.Select(a => new UserModel
            {
                UserId = a.UserId,
                Name = a.Name,
                Surname = a.Surname,
                Patronymic = a.Patronymic,
                Login = a.Login,
                Password = a.Password,
                Role = new RoleModel()
                {
                    RoleId = a.RoleId,
                    Name = a.Role.Name
                }
            }).ToListAsync();
            return users;
        }

        [HttpPut("update-user")]
        public async Task UpdateUser([FromBody] UserUpdateInfo userModel)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(a => a.UserId == userModel.UserId);
            user.Name = userModel.Name;
            user.Surname = userModel.Surname;
            user.Patronymic = userModel.Patronymic;
            user.Login = userModel.Login;
            user.Password = userModel.Password;
            user.RoleId = userModel.RoleId;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        [HttpDelete("del-user/{userid}")]
        public async Task DeleteUser([FromRoute] int userid)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(a => a.UserId == userid);
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}