using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using UserManagement.Common.Models;
using UserManagement.Manager.Interfaces;
using UserManagement.Common.ModelsAPI;
using System.Linq;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        #region Members  
        private const string ExistUserName = "Please check your user name.";
        private const string ExistEmail = "Please check your email.";
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        #endregion

        #region Constructor  
        public UsersController(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userManager = userManager;
        }
        #endregion

        #region Methods  
        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody]TokenRequest request)
        {
            try
            {
                var user = _userRepository.Authenticate(request.Username, request.Password);

                if (user == null)
                    return BadRequest(new { message = "Username or password is incorrect." });
                else if (user.UserRole.Any(x => x.Name == Role.User))
                    return BadRequest(new { message = "User Student can't login." });
                else if (user.Status == Status.InProgress)
                    return BadRequest(new { message = "User is not accepted yet." });
                else if (user.Status == Status.Reject)
                    return BadRequest(new { message = "User is rejected." });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); ;
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                bool isEditable = false;
                var users = _userRepository.GetAll();
                if (User.IsInRole(Role.Admin))
                    isEditable = true;

                return Ok(new { users, IsEditable = isEditable });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); ;
            }
        }

        [HttpGet]
        [Route("GetById")]
        public IActionResult GetById(int id)
        {
            try
            {
                var user = _userRepository.GetById(id);

                if (user == null)
                {
                    return NotFound();
                }

                // only allow admins to access other user records
                var currentUserId = int.Parse(User.Identity.Name);
                if (id != currentUserId && !User.IsInRole(Role.Admin))
                {
                    return Forbid();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); ;
            }
        }

        [HttpPost]
        [Route("SaveUserStatus")]
        public IActionResult SaveUserStatus(int id, [FromBody] UserAPI user)
        {
            try
            {
                if (User.IsInRole(Role.Admin))
                {
                    var item = _userRepository.UpdateUserStatus(user);

                    return Ok(item);
                }
                return BadRequest("User Authentication not allowed for changing status");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); ;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserAPI user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var checkUserNameuser = await _userManager.FindByNameAsync(user.UserName);
                var checkEmailUser = await _userManager.FindByEmailAsync(user.Email);

                if (checkUserNameuser != null && checkEmailUser != null)
                    return Ok(new { UserExist = true });

                if (checkUserNameuser != null)
                    return StatusCode(StatusCodes.Status400BadRequest, ExistUserName);

                if (checkEmailUser != null)
                    return StatusCode(StatusCodes.Status400BadRequest, ExistEmail);

                var userItem = new User
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password,
                    Token = user.Token,
                    Status = user.Status,
                    UserName = user.UserName,
                };
                //add user
                var item = await _userManager.CreateAsync(userItem, user.Password);
                //add user role
                var role = await _userManager.AddToRoleAsync(userItem, Role.User); 

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); ;
            }
        }
        #endregion
    }
}
