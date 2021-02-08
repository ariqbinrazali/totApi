using api2.Data;
using api2.Models;
using api2.Models.Dtos;
using api2.Models.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace api2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(400)]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;
        private readonly IUserRepository userRepo;

        public UserController(ApplicationDBContext db, IMapper mapper, IUserRepository userRepo)
        {
            _db = db;
            _mapper = mapper;
            this.userRepo = userRepo;
        }

        [HttpGet(Name = nameof(GetAllUser))]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(404)]

        public IActionResult GetAllUser()
        {
            IEnumerable<User> obj = _db.Users.OrderBy(x => x.Username).ToList();

            if (obj == null)
            {
                return NotFound();
            }

            return Ok(obj);

        }

        [HttpGet("{UserId:int}", Name = nameof(GetUser))]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]

        public IActionResult GetUser(int UserId)
        {
            var obj = _db.Users.FirstOrDefault(x => x.Id == UserId);
            if (obj == null)
            {
                return NotFound();

            }

            return Ok(obj);
        }
        [AllowAnonymous]
        [HttpPost(Name = nameof(CreateUser))]
        [ProducesResponseType(201, Type = typeof(User))]

        public IActionResult CreateUser(CreateUserDto createUserDto)
        {
            createUserDto.Password = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
            bool isExist = _db.Users.Any(x => x.Username.ToLower().Trim() == createUserDto.Username.ToLower().Trim());

            if (createUserDto == null)
            {
                return BadRequest();
            }
            if (isExist)
            {
                ModelState.AddModelError("", "Username  exists");
                return StatusCode(400, ModelState);
            }

            var obj = _mapper.Map<User>(createUserDto);
            _db.Users.Add(obj);
            _db.SaveChanges();
            return CreatedAtRoute(nameof(GetUser), new { userId = obj.Id }, obj);
        }

        [HttpPatch(Name = nameof(UpdateUser))]
        [ProducesResponseType(204, Type = typeof(User))]

        public IActionResult UpdateUser(String userName, CreateUserDto createUserDto)
        {
            bool isExist = _db.Users.Any(x => x.Username.ToLower().Trim() == createUserDto.Username.ToLower().Trim());

            if (createUserDto == null && createUserDto.Username != userName)
            {
                return BadRequest();
            }

            if (isExist)
            {
                ModelState.AddModelError("", "Username exist");
                return StatusCode(400, ModelState);
            }
            var obj = _mapper.Map<User>(createUserDto);
            _db.Users.Update(obj);
            _db.SaveChanges();
            return CreatedAtRoute(nameof(GetUser), new { UserId = obj.Id }, obj);


        }
        [HttpDelete(Name = nameof(DeleteUser))]
        [ProducesResponseType(204, Type = typeof(User))]

        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public IActionResult DeleteUser(int id)
        {
            var obj = _db.Users.FirstOrDefault(x => x.Id == id);
            if (obj.Id != id)
            {
                return NotFound();
            }
            _db.Users.Remove(obj);
            _db.SaveChanges();

            return NoContent();
        }
        [AllowAnonymous]
        [HttpPost("/Authentication")]
        [ProducesResponseType(200,Type =typeof(UserSignInDto))]
        public IActionResult Authenticate(UserSignInDto model)
        {
            if (ModelState.IsValid)
            {
                var user = userRepo.Authenticate(model);
                return Ok(user);
            }

            return BadRequest(new { message = "NotValid" });

        }
    }
}
