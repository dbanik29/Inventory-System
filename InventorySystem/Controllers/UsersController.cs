using InventorySystem.Model;
using InventorySystem.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace InventorySystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(User admin)
        {
            var useradmin = _userService.LoginAuthentication(admin.UserName, admin.Password);

            if (useradmin == null)
                return BadRequest(new { message = "Username or Password is incorrect" });

            return Ok(useradmin);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("UserRegistration")]
        public async Task<IActionResult> UserRegistration([FromBody]User model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var modelid = await _userService.AddUser(model);
                    if (modelid > 0)
                    {
                        return Ok(modelid);
                    }
                    else
                    {
                        return BadRequest(new { message = "User registration Failed" });
                    }
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }
            
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetAllUser")]
        public async Task<IActionResult> GetAllUser()
        {           
            try
            {
                List<User> euser = new List<User>();
                var users = await _userService.GetAllUser();
                foreach (var model in users)
                {
                    if (model.UserId % 2 == 0)
                    {
                        //List<User> aduser = new List<User>();
                        euser.Add(model);
                    }
                }
                if (euser == null)
                {
                    return NotFound();
                }
                return Ok(euser);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Authorize(Roles = "Manager")]
        [Route("GetOddUsers")]
        public async Task<IActionResult> GetOddUsers()
        {
            try
            {
                List<User> odduser = new List<User>();
                var users = await _userService.GetAllUser();
                foreach (var model in users)
                {
                    if (model.UserId % 2 == 1)
                    {
                        odduser.Add(model);
                    }
                }
                if (odduser == null)
                {
                    return NotFound();
                }
                return Ok(odduser);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("AddUser")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> AddUser([FromBody]User model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var usr = await _userService.AddUser(model);
                    if (usr > 0)
                    {
                        return Ok(usr);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int? UserId)
        {
            int result = 0;
            if (UserId == null)
            {
                return BadRequest();
            }
            try
            {
                result = await _userService.DeleteUser(UserId);
                if (result == 0)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody]User model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userService.UpdateUser(model);

                    return Ok("Success");
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }
    }
}
