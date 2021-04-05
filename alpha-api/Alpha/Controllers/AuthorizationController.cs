using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Alpha.DataBase;
using Alpha.DataBase.Entities;
using Alpha.Models.Login;
using Alpha.Models.Role;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Alpha.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthorizationController : Controller
    {
        private readonly IConfiguration _config;
        private readonly AlphaDBContext _dbContext;


        public AuthorizationController(IConfiguration config, AlphaDBContext dbContext)
        {
            _config = config;
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult CreateToken([FromBody] LoginModel login)
        {
            if (login == null) return Unauthorized();
            string tokenString = string.Empty;
            var validUser = Authenticate(login);
            if (validUser != null)
            {
                tokenString = BuildJwtToken(validUser);
            }
            else
            {
                return Unauthorized();
            }

            return Ok(new {Token = tokenString});
        }

        private string BuildJwtToken(LoginAuthorize authorize)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtToken:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var issuer = _config["JwtToken:Issuer"];
            var audience = _config["JwtToken:Audience"];
            var jwtValidity = DateTime.Now.AddMinutes(Convert.ToDouble(_config["JwtToken:TokenExpiry"]));
            var claims = new List<Claim>()
            {
                new Claim("UserId", authorize.UserId.ToString()),
                new Claim("Login", authorize.Login),
                new Claim(ClaimTypes.Role, authorize.Role.Name),
            };

            var token = new JwtSecurityToken(issuer, audience, claims, expires: jwtValidity, signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private LoginAuthorize Authenticate(LoginModel login)
        {
            var validUser = _dbContext.Users.Where(a => a.Login == login.Login && a.Password == login.Password)
                .Select(a => new LoginAuthorize
                {
                    UserId = a.UserId, Login = a.Login, Role = new RoleModel() {Name = a.Role.Name, RoleId = a.RoleId}
                })
                .SingleOrDefault();

            return validUser;
        }
    }
}