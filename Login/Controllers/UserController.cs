using Login.DataContext;
using Login.Migrations;
using Login.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Login.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly APIDbContext _context;
        private readonly IConfiguration _configuration;
        public UserController(APIDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration(Users user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var obj = _context.Users.FirstOrDefault(x => x.Email == user.Email);
            if (obj == null)
            {
                _context.Users.Add(new Users
                {
                    UserName = user.UserName,
                    Password = user.Password,
                    Email = user.Email,


                });
                _context.SaveChanges();
                return Ok("User Created");
            }

            else
            {
                return BadRequest("Already exixts");
            }


        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(Users login)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == login.Email && x.Password == login.Password);



            if (user != null)
            {
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("User Id", user.UserId.ToString()),
                new Claim("Email", user.Email.ToString())
        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(5),
                    signingCredentials: signIn
                );

                string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

                //Create a UserViewModel instance

                var userViewModel = new Users
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    UserName = user.UserName // Include any other fields you want to return

                };


                return Ok(new { token = tokenValue, user = userViewModel });
                //return Ok(new { token = tokenValue });


            }
            return NoContent();
            //return BadRequest(ModelState);
        }


        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            return Ok(_context.Users.ToList());

        }
        //[Authorize]
        //[HttpGet]

        //[Route("GetUser")]

        //public IActionResult GetUser(int id)
        //{
        //    var obj = _context.Users.FirstOrDefault(x => x.UserId == id);

        //    if (obj != null)
        //    {
        //        return Ok(obj);
        //    }
        //    else
        //    {
        //        return NoContent();
        //    }

        //}
        [Authorize]
        [HttpGet]

        [Route("GetUser")]

        public IActionResult GetUser()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                return BadRequest("Authorization header not found.");
            }

            var token = authorizationHeader.ToString().Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            var userId = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "User Id").Value;

            if (userId != null)
            {

                var obj = _context.Users.FirstOrDefault(x => x.UserId.ToString() == userId);

                
                return Ok(obj);


            }
            return Ok();
        }

    }
}