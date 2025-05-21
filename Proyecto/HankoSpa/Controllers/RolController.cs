using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using HankoSpa.Helpers;
using HankoSpa.Services.Interfaces;
using HankoSpa.DTOs;
using HankoSpa.Attributes;
using System.Collections.Generic;
using AspNetCoreHero.ToastNotification.Notyf.Models;
using HankoSpa.Data;
using Microsoft.EntityFrameworkCore;
using HankoSpa.Models;
using System.Linq;
using System.Threading.Tasks;

namespace HankoSpa.Controllers
{
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
        [CustomAuthorize(permission: "showRoles", module: "Roles")]
        public async Task<IActionResult> Index()
        {
            var response = await _rolService.GetAllAsync();
            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                return View(new List<CustomRolDTO>());
            }
            return View(response.Result);
        }

        [HttpGet]
        [CustomAuthorize(permission: "createRoles", module: "Roles")]
        public IActionResult Create()
        {
            var dto = new CustomRolDTO();
            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permission: "createRoles", module: "Roles")]
        public async Task<IActionResult> Create(CustomRolDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe ajustar los errores de validación");
                return View(dto);
            }

            var response = await _rolService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                return View(dto);
            }

            // Asignación automática de permisos según el nombre del rol
            var rol = await _context.CustomRoles.FirstOrDefaultAsync(r => r.NombreRol == dto.NombreRol);
            if (rol != null)
            {
                var allPerms = await _context.Permissions.ToListAsync();
                if (rol.NombreRol == "SuperUser")
                {
                    foreach (var perm in allPerms)
                    {
                        if (!_context.RolPermissions.Any(rp => rp.CustomRolId == rol.CustomRolId && rp.PermissionId == perm.PermisoId))
                            _context.RolPermissions.Add(new RolPermission { CustomRolId = rol.CustomRolId, PermissionId = perm.PermisoId });
                    }
                }
                else if (rol.NombreRol == "Empleado")
                {
                    foreach (var perm in allPerms.Where(p => p.Module == "Servicios" || p.Module == "Citas"))
                    {
                        if (!_context.RolPermissions.Any(rp => rp.CustomRolId == rol.CustomRolId && rp.PermissionId == perm.PermisoId))
                            _context.RolPermissions.Add(new RolPermission { CustomRolId = rol.CustomRolId, PermissionId = perm.PermisoId });
                    }
                }
                else if (rol.NombreRol == "Cliente")
                {
                    foreach (var perm in allPerms.Where(p => p.Module == "Citas"))
                    {
                        if (!_context.RolPermissions.Any(rp => rp.CustomRolId == rol.CustomRolId && rp.PermissionId == perm.PermisoId))
                            _context.RolPermissions.Add(new RolPermission { CustomRolId = rol.CustomRolId, PermissionId = perm.PermisoId });
                    }
                }
                await _context.SaveChangesAsync();
            }

            _notifyService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [CustomAuthorize(permission: "updateRoles", module: "Roles")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
                return NotFound();

            var response = await _rolService.GetByIdAsync(id);

            if (!response.IsSuccess || response.Result == null)
                return NotFound();

            var dto = response.Result;
            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permission: "updateRoles", module: "Roles")]
        public async Task<IActionResult> Edit(CustomRolDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe ajustar los errores de validación");
                return View(dto);
            }

            var response = await _rolService.UpdateAsync(dto);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                return View(dto);
            }

            _notifyService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}


