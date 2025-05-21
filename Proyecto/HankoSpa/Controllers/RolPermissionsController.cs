using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using HankoSpa.Services.Interfaces;
using HankoSpa.DTOs;

namespace HankoSpa.Controllers
{
    public class RolPermissionsController : Controller
    {
        private readonly IRolPermissionService _rolPermissionService;
        private readonly ICustomRolService _rolService;
        private readonly IPermissionService _permissionService;
        private readonly INotyfService _notifyService;

        public RolPermissionsController(
            IRolPermissionService rolPermissionService,
            ICustomRolService rolService,
            IPermissionService permissionService,
            INotyfService notifyService)
        {
            _rolPermissionService = rolPermissionService;
            _rolService = rolService;
            _permissionService = permissionService;
            _notifyService = notifyService;
        }

        // Lista los permisos asignados a un rol
        [HttpGet]
        public async Task<IActionResult> Index(int rolId)
        {
            var rolResponse = await _rolService.GetByIdAsync(rolId);
            if (!rolResponse.IsSuccess || rolResponse.Result == null)
            {
                _notifyService.Error("Rol no encontrado.");
                return RedirectToAction("Index", "Rol");
            }

            var response = await _rolPermissionService.GetPermissionsByRolIdAsync(rolId);
            ViewBag.Rol = rolResponse.Result;
            return View(response.Result ?? new List<RolPermissionDTO>());
        }

        // Muestra formulario para asignar un permiso a un rol
        [HttpGet]
        public async Task<IActionResult> Assign(int rolId)
        {
            var permissionsResponse = await _permissionService.GetAllAsync();
            var dto = new RolPermissionDTO
            {
                CustomRolId = rolId
            };
            ViewBag.Permissions = permissionsResponse.Result ?? new List<PermissionDTO>();
            return View(dto);
        }

        // Asigna un permiso a un rol
        [HttpPost]
        public async Task<IActionResult> Assign(RolPermissionDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var permissionsResponse = await _permissionService.GetAllAsync();
                ViewBag.Permissions = permissionsResponse.Result ?? new List<PermissionDTO>();
                _notifyService.Error("Debe ajustar los errores de validación");
                return View(dto);
            }

            var response = await _rolPermissionService.AssignPermissionAsync(dto);

            if (!response.IsSuccess)
            {
                var permissionsResponse = await _permissionService.GetAllAsync();
                ViewBag.Permissions = permissionsResponse.Result ?? new List<PermissionDTO>();
                _notifyService.Error(response.Message);
                return View(dto);
            }

            _notifyService.Success(response.Message);
            return RedirectToAction(nameof(Index), new { rolId = dto.CustomRolId });
        }

        // Elimina un permiso de un rol
        [HttpPost]
        public async Task<IActionResult> Remove(int rolId, int permissionId)
        {
            var response = await _rolPermissionService.RemovePermissionAsync(rolId, permissionId);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
            }
            else
            {
                _notifyService.Success(response.Message);
            }

            return RedirectToAction(nameof(Index), new { rolId });
        }
    }
}

