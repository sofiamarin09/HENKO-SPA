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
            var model = new UserDTO();
            await GetRolesAvailables(model);
            return View(model);
        }


        // roles disponibles
        private async Task GetRolesAvailables(UserDTO userDTO)
        {
            var roleDTOs = await _userService.GetAllRolesAsync();
            userDTO.CustomRoles = roleDTOs.Select(r => new SelectListItem
            {
                Value = r.CustomRolId.ToString(),
                Text = r.NombreRol
            }).ToList();
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

        // POST: User/Create	
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                await GetRolesAvailables(userDTO);
                return View(userDTO);
            }

            var result = await _userService.CreateUserAsync(userDTO, userDTO.Password!);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            await GetRolesAvailables(userDTO);
            return View(userDTO);
        }



    }

}