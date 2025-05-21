using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using HankoSpa.Services.Interfaces;
using HankoSpa.DTOs;
using HankoSpa.Controllers.Attributes;

namespace HankoSpa.Controllers
{
    public class PermissionsController : Controller
    {
        private readonly IPermissionService _permissionService;
        private readonly INotyfService _notifyService;
        private readonly IMapper _mapper;

        public PermissionsController(IPermissionService permissionService, INotyfService notifyService, IMapper mapper)
        {
            _permissionService = permissionService;
            _notifyService = notifyService;
            _mapper = mapper;
        }

        [HttpGet]
        [CustomAuthorize(permission: "showPermissions", module: "Permisos")]
        public async Task<IActionResult> Index()
        {
            var response = await _permissionService.GetAllAsync();
            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                return View(new List<PermissionDTO>());
            }
            return View(response.Result);
        }

        [HttpGet]
        [CustomAuthorize(permission: "createPermissions", module: "Permisos")]
        public IActionResult Create()
        {
            var dto = new PermissionDTO();
            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permission: "createPermissions", module: "Permisos")]
        public async Task<IActionResult> Create(PermissionDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe ajustar los errores de validación");
                return View(dto);
            }

            var response = await _permissionService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                return View(dto);
            }

            _notifyService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [CustomAuthorize(permission: "updatePermissions", module: "Permisos")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
                return NotFound();

            var response = await _permissionService.GetByIdAsync(id);

            if (!response.IsSuccess || response.Result == null)
                return NotFound();

            var dto = response.Result;
            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permission: "updatePermissions", module: "Permisos")]
        public async Task<IActionResult> Edit(PermissionDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe ajustar los errores de validación");
                return View(dto);
            }

            var response = await _permissionService.UpdateAsync(dto);

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

