using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using VisitsSystem.Dto;
using VisitsSystem.Interfaces;
using VisitsSystem.Models;

namespace VisitsSystem.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        static SymmetricSecurityKey ke;
        private readonly IUserRepository _usersRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public UserController(IUserRepository usersRepository,
            IConfiguration configuration,IMapper mapper)
        {
            _mapper = mapper;
            _usersRepository = usersRepository;
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            if (!_usersRepository.UserExists(id))
                return NotFound();
            var user = _usersRepository.GetUser(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(user);
        }
        [HttpGet("[action]")]
        public IActionResult GetAllUsers()
        {
            List<User> users = _usersRepository.GetAllUsers();
            if (users == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<List<UserDto>>(users));
        }
        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (_usersRepository.UsernameExists(user.UserName))
            {
                ModelState.AddModelError("", "هذا المستخدم موجود بالفعل  ");
                return StatusCode(500, ModelState);
            }
            if (!_usersRepository.CreateUser(user))
            {
                ModelState.AddModelError("", "حدث خطأ في إضافة المستخدم ");
                return StatusCode(500, ModelState);
            }
            return Ok(user);
        }

        [HttpPut("ResetPassword/{UserID}")]
        public IActionResult ResetPassword([FromRoute] int UserID, [FromBody] String password)
        {
            if (password == null)
            {
                return BadRequest();
            }

            if (!_usersRepository.UserExists(UserID))
                return NotFound();
            User user = _usersRepository.GetUser(UserID);
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            password = Convert.ToHexString(hashBytes);
            user.HashedPassword = password;
            if (!_usersRepository.UpdateUser(user))
            {
                ModelState.AddModelError("", "حدث خطأ في تحديث بيانات المستخدم ");
                return StatusCode(500, ModelState);
            }
            return Ok(user);
        }

        [HttpDelete]
        public IActionResult DeleteUser(int userID)
        {
            if (userID == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = _usersRepository.GetUser(userID);

            if (!_usersRepository.DeleteUser(user))
            {
                ModelState.AddModelError("", "حدث خطأ في حذف المستخدم ");
                return StatusCode(500, ModelState);
            }
            return Ok(user);
        }


        [HttpPost, Route("[action]", Name = "User")]
        public async Task<IActionResult> UserLoginAsync([FromBody] UserDto User)
        {
            if (!_usersRepository.UserLogIn(User.UserName, User.HashedPassword))
                return NotFound();
            int userid = _usersRepository.GetUserID(User.UserName, User.HashedPassword);
            var user = _usersRepository.GetUser(userid);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, userid.ToString())
                };


            var hash = new HMACSHA512();

            user.Hash = Convert.ToBase64String(hash.Key);
            hash = new HMACSHA512(System.Text.Encoding.UTF8.GetBytes(user.Hash));

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(claims: claims, signingCredentials: creds, expires: DateTime.Now.AddDays(1));
            _usersRepository.Save();

            string Token = new JwtSecurityTokenHandler().WriteToken(token);
            string id = Convert.ToBase64String(hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(user.UserId.ToString())));
            return Ok(new
            {
                Token = Token,
                id = id,
                user = _mapper.Map<UserDto>(user)
            });
        }

        [HttpGet("Logout"), Authorize]
        public async Task<IActionResult> Logout([FromHeader] int ID, [FromHeader] string hashid)
        {
            string ret = _usersRepository.LogIn(ID, hashid);
            if (ret != "")
            {
                return BadRequest(ret);
            }
            User user = _usersRepository.GetUser(ID);
            user.Hash = null;
            _usersRepository.Save();

            return Ok();


        }
    }
}
