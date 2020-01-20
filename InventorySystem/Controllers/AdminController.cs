using InventorySystem.Model;
using InventorySystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        public readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // GET api/values
        [HttpGet]
        [Route("GetAll")]
        [Authorize(Roles = "Admin")]
        public ActionResult GetAll()
        {
            try
            {
                var model = _adminService.GetAll();
                if (model == null)
                {
                    return NotFound();
                }

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult Authenticate([FromBody]Admin admin)
        {
            var useradmin = _adminService.Authenticate(admin.UserName, admin.Password);

            if (useradmin == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(useradmin);
        }


        [HttpPost]
        [Route("AddAdmin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddAdmin([FromBody]Admin model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var adn = _adminService.AddAdmin(model);
                    if (adn > 0)
                    {
                        return Ok(adn);
                    }
                    else
                    {
                        return BadRequest(new { message = "User creation failed" });
                    }
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

