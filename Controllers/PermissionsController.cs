﻿using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HankoSpa.Services.Interfaces;
using HankoSpa.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HankoSpa.Controllers
{
    [Authorize] // Solo usuarios autenticados pueden acceder a este controlador
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
        [Authorize(Policy = "Permiso_Read")]
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
        [Authorize(Policy = "Permiso_Read")]
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
                return NotFound();

            var response = await _permissionService.GetByIdAsync(id);

            if (!response.IsSuccess || response.Result == null)
                return NotFound();

            var dto = response.Result;
            return View(dto);
        }

        [HttpGet]
        [Authorize(Policy = "PermisoCRUD")]
        public IActionResult Create()
        {
            var dto = new PermissionDTO();
            return View(dto);
        }

        [HttpPost]
        [Authorize(Policy = "PermisoCRUD")]
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
        [Authorize(Policy = "PermisoCRUD")]
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
        [Authorize(Policy = "PermisoCRUD")]
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

        // Acción para mostrar confirmación de eliminación
        [HttpGet]
        [Authorize(Policy = "PermisoCRUD")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
                return NotFound();

            var response = await _permissionService.GetByIdAsync(id);

            if (!response.IsSuccess || response.Result == null)
                return NotFound();

            var dto = response.Result;
            return View(dto);
        }

        // Acción para eliminar (POST)
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "PermisoCRUD")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _permissionService.DeleteAsync(id);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            _notifyService.Success("Permiso eliminado correctamente");
            return RedirectToAction(nameof(Index));
        }
    }
}
