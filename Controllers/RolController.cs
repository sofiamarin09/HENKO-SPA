using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HankoSpa.Helpers;
using HankoSpa.Services.Interfaces;
using HankoSpa.DTOs;
using System.Collections.Generic;
using AspNetCoreHero.ToastNotification.Notyf.Models;
using HankoSpa.Data;
using Microsoft.EntityFrameworkCore;
using HankoSpa.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HankoSpa.Controllers
{
    [Authorize] // Solo usuarios autenticados pueden acceder a este controlador
    public class RolController : Controller
    {
        private readonly ICustomRolService _rolService;
        private readonly INotyfService _notifyService;
        private readonly ICombosHelper _combosHelper;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public RolController(
            ICustomRolService rolService,
            INotyfService notifyService,
            ICombosHelper combosHelper,
            IMapper mapper,
            AppDbContext context)
        {
            _rolService = rolService;
            _notifyService = notifyService;
            _combosHelper = combosHelper;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [Authorize(Policy = "Rol_Read")]
        public async Task<IActionResult> Index()
        {
            var response = await _rolService.GetAllAsync();
            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                return View("~/Views/CustomRol/Index.cshtml", new List<CustomRolDTO>());
            }
            return View("~/Views/CustomRol/Index.cshtml", response.Result);
        }

        [HttpGet]
        [Authorize(Policy = "RolCRUD")]
        public async Task<IActionResult> Create()
        {
            var permisos = await _context.Permissions.ToListAsync();
            var model = new CreateRolViewModel
            {
                AllPermissions = permisos
                    .Select(p => new SelectListItem
                    {
                        Value = p.PermisoId.ToString(),
                        Text = p.NombrePermiso
                    }).ToList()
            };
            return View("~/Views/CustomRol/Create.cshtml", model);
        }

        [HttpPost]
        [Authorize(Policy = "RolCRUD")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRolViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Recargar permisos si hay error
                var permisos = await _context.Permissions.ToListAsync();
                model.AllPermissions = permisos
                    .Select(p => new SelectListItem
                    {
                        Value = p.PermisoId.ToString(),
                        Text = p.NombrePermiso
                    }).ToList();
                _notifyService.Error("Debe ajustar los errores de validación");
                return View("~/Views/CustomRol/Create.cshtml", model);
            }

            // Crear el rol
            var rol = new CustomRol { NombreRol = model.NombreRol };
            _context.CustomRoles.Add(rol);
            await _context.SaveChangesAsync();

            // Asignar permisos seleccionados
            if (model.SelectedPermissionIds != null)
            {
                foreach (var permId in model.SelectedPermissionIds)
                {
                    _context.RolPermissions.Add(new RolPermission
                    {
                        CustomRolId = rol.CustomRolId,
                        PermissionId = permId
                    });
                }
                await _context.SaveChangesAsync();
            }

            _notifyService.Success("Rol creado y permisos asignados correctamente");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Policy = "RolCRUD")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
                return NotFound();

            // Cargar el rol y sus permisos asignados
            var rol = await _context.CustomRoles
                .Include(r => r.RolPermissions)
                .FirstOrDefaultAsync(r => r.CustomRolId == id);

            if (rol == null)
                return NotFound();

            var permisos = await _context.Permissions.ToListAsync();

            var model = new CreateRolViewModel
            {
                NombreRol = rol.NombreRol,
                SelectedPermissionIds = rol.RolPermissions.Select(rp => rp.PermissionId).ToList(),
                AllPermissions = permisos
                    .Select(p => new SelectListItem
                    {
                        Value = p.PermisoId.ToString(),
                        Text = p.NombrePermiso
                    }).ToList()
            };

            ViewBag.CustomRolId = rol.CustomRolId;
            return View("~/Views/CustomRol/Edit.cshtml", model);
        }

        [HttpPost]
        [Authorize(Policy = "RolCRUD")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateRolViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Recargar permisos si hay error
                var permisos = await _context.Permissions.ToListAsync();
                model.AllPermissions = permisos
                    .Select(p => new SelectListItem
                    {
                        Value = p.PermisoId.ToString(),
                        Text = p.NombrePermiso
                    }).ToList();
                ViewBag.CustomRolId = id;
                _notifyService.Error("Debe ajustar los errores de validación");
                return View("~/Views/CustomRol/Edit.cshtml", model);
            }

            var rol = await _context.CustomRoles
                .Include(r => r.RolPermissions)
                .FirstOrDefaultAsync(r => r.CustomRolId == id);

            if (rol == null)
                return NotFound();

            rol.NombreRol = model.NombreRol;

            // Actualizar permisos asignados
            var nuevosPermisos = model.SelectedPermissionIds ?? new List<int>();

            // Eliminar permisos actuales de la base de datos
            var permisosActuales = _context.RolPermissions.Where(rp => rp.CustomRolId == id).ToList();
            _context.RolPermissions.RemoveRange(permisosActuales);

            // Agregar los nuevos permisos seleccionados
            foreach (var permisoId in nuevosPermisos)
            {
                _context.RolPermissions.Add(new RolPermission { CustomRolId = id, PermissionId = permisoId });
            }

            await _context.SaveChangesAsync();

            _notifyService.Success("Rol actualizado correctamente");
            return RedirectToAction(nameof(Index));
        }

        // Acción para ver detalles
        [HttpGet]
        [Authorize(Policy = "RolCRUD")]
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
                return NotFound();

            // Cargar el rol con sus permisos y los datos de cada permiso
            var rol = await _context.CustomRoles
                .Include(r => r.RolPermissions)
                    .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.CustomRolId == id);

            if (rol == null)
                return NotFound();

            // Mapeo explícito para asegurar que los permisos se incluyan en el DTO
            var dto = new CustomRolDTO
            {
                CustomRolId = rol.CustomRolId,
                NombreRol = rol.NombreRol,
                RolPermissions = rol.RolPermissions
                    .Select(rp => new RolPermission
                    {
                        CustomRolId = rp.CustomRolId,
                        PermissionId = rp.PermissionId,
                        Permission = rp.Permission
                    }).ToList()
            };

            return View("~/Views/CustomRol/Details.cshtml", dto);
        }

        // Acción para mostrar confirmación de eliminación
        [HttpGet]
        [Authorize(Policy = "RolCRUD")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
                return NotFound();

            var response = await _rolService.GetByIdAsync(id);

            if (!response.IsSuccess || response.Result == null)
                return NotFound();

            var dto = response.Result;
            return View("~/Views/CustomRol/Delete.cshtml", dto);
        }

        // Acción para eliminar (POST)
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "RolCRUD")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _rolService.DeleteAsync(id);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            _notifyService.Success("Rol eliminado correctamente");
            return RedirectToAction(nameof(Index));
        }
    }
}


