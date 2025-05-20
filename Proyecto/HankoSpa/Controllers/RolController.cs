using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using HankoSpa.Helpers;
using HankoSpa.Services.Interfaces;
using HankoSpa.DTOs;
using HankoSpa.Attributes;
using System.Collections.Generic;
using AspNetCoreHero.ToastNotification.Notyf.Models;

namespace HankoSpa.Controllers
{
    public class RolController : Controller
    {
        private readonly ICustomRolService _rolService;
        private readonly INotyfService _notifyService;
        private readonly ICombosHelper _combosHelper;
        private readonly IMapper _mapper;

        public RolController(ICustomRolService rolService, INotyfService notifyService,ICombosHelper combosHelper, IMapper mapper)
        {
            _rolService = rolService;
            _notifyService = notifyService;
            _combosHelper = combosHelper;
            _mapper = mapper;
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
