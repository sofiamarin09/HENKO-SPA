using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using HankoSpa.Services.Interfaces;
using HankoSpa.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HankoSpa.Controllers
{
    [Authorize] // Solo usuarios autenticados pueden acceder a este controlador
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // READ: Listado de usuarios
        [HttpGet]
        [Authorize(Policy = "Usuarios_Read")]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        // CREATE: Mostrar formulario
        [HttpGet]
      
        [Authorize(Policy = "Usuarios_Read")]

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

        // UPDATE: Mostrar formulario
        [HttpGet]
        [Authorize(Policy = "UsuarioCRUD")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            await GetRolesAvailables(user);
            return View(user);
        }

        // DELETE: Mostrar confirmación
        [HttpGet]
        [Authorize(Policy = "UsuarioCRUD")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // CREATE: Guardar usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "UsuarioCRUD")]
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

        // UPDATE: Guardar cambios
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "UsuarioCRUD")]
        public async Task<IActionResult> Edit(UserDTO userDTO)
        {
            if (string.IsNullOrEmpty(userDTO.Id) || !Guid.TryParse(userDTO.Id, out var userId))
            {
                ModelState.AddModelError(string.Empty, "Id de usuario inválido.");
                await GetRolesAvailables(userDTO);
                return View(userDTO);
            }

            ModelState.Remove(nameof(userDTO.Password));
            ModelState.Remove(nameof(userDTO.ConfirmPassword));

            if (!ModelState.IsValid)
            {
                await GetRolesAvailables(userDTO);
                return View(userDTO);
            }

            userDTO.Password = null;
            userDTO.ConfirmPassword = null;

            var updated = await _userService.UpdateUserAsync(userDTO);
            if (!updated)
            {
                ModelState.AddModelError(string.Empty, "Error al actualizar el usuario.");
                await GetRolesAvailables(userDTO);
                return View(userDTO);
            }

            return RedirectToAction(nameof(Index));
        }

        // DELETE: Confirmar eliminación
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "UsuarioCRUD")]
        public async Task<IActionResult> Delete(UserDTO userDTO)
        {
            if (string.IsNullOrEmpty(userDTO.Id) || !Guid.TryParse(userDTO.Id, out var userId))
            {
                return NotFound();
            }

            var deleted = await _userService.DeleteUserAsync(userId);
            if (!deleted)
            {
                ModelState.AddModelError(string.Empty, "No se pudo eliminar el usuario.");
                return View(userDTO);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
