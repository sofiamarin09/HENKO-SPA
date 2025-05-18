using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HankoSpa.Services.Interfaces;
using HankoSpa.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HankoSpa.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        // GET: User
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roleDTOs = await _userService.GetAllRolesAsync();

            var model = new UserDTO
            {
                CustomRoles = roleDTOs.Select(r => new SelectListItem
                {
                    Value = r.CustomRolId.ToString(),
                    Text = r.NombreRol
                }).ToList()
            };

            return View(model);
        }

        // Get: User/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // Get: User/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

    }

}