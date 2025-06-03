using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization; // <-- Agregado para control de acceso
using HankoSpa.Services.Interfaces;
using HankoSpa.DTOs;

namespace HankoSpa.Controllers
{
    [Authorize] // <-- Control de acceso: solo usuarios autenticados pueden acceder a este controlador
    public class ServicioController : Controller
    {
        private readonly IServicioServices _servicioService;

        public ServicioController(IServicioServices servicioService)
        {
            _servicioService = servicioService;
        }

        // GET: Servicio
        [Authorize(Policy = "Servicios_Read")]
        public async Task<IActionResult> Index()
        {
            var response = await _servicioService.GetAllAsync();

            if (!response.IsSuccess)
            {
                ViewBag.ErrorMessage = response.Message;
                return View(new List<ServiceDTO>());
            }

            return View(response.Result);
        }

        // GET: Servicio/Create
        [Authorize(Policy = "Servicios_Create")]
        public IActionResult Create()
        {
            return View();
        }

        // GET: Servicio/Edit
        [Authorize(Policy = "Servicios_Update")]
        public IActionResult Edit()
        {
            return View();
        }

        // GET: Servicio/Delete
        [Authorize(Policy = "Servicios_Delete")]
        public IActionResult Delete()
        {
            return View();
        }

        // POST: Servicio/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Servicios_Create")]
        public async Task<IActionResult> Create(ServiceDTO servicioDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _servicioService.CreateAsync(servicioDTO);

                if (response.IsSuccess)
                {
                    TempData["MensajeExito"] = "Servicio creado exitosamente.";
                    return RedirectToAction("Index");
                }

                ViewBag.ErrorMessage = response.Message;
            }

            return View(servicioDTO);
        }

        // POST: Servicio/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Servicios_Update")]
        public async Task<IActionResult> Edit(int id, ServiceDTO servicioDTO)
        {
            if (id != servicioDTO.ServicioId) return NotFound();

            if (ModelState.IsValid)
            {
                var response = await _servicioService.EditAsync(servicioDTO);
                if (response.IsSuccess)
                {
                    TempData["MensajeExito"] = "Servicio actualizado exitosamente.";
                    return RedirectToAction("Index");
                }

                ViewBag.ErrorMessage = response.Message;
            }

            return View(servicioDTO);
        }

        // GET: Servicio/Edit/5
        [HttpGet]
        [Authorize(Policy = "Servicios_Update")]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _servicioService.GetOneAsync(id);
            if (!response.IsSuccess || response.Result == null)
            {
                TempData["MensajeError"] = "El servicio no fue encontrado.";
                return RedirectToAction("Index");
            }

            return View(response.Result);
        }

        // GET: Servicio/Delete/5
        [HttpGet]
        [Authorize(Policy = "Servicios_Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var response = await _servicioService.GetOneAsync(id.Value);
            if (!response.IsSuccess || response.Result == null)
            {
                TempData["MensajeError"] = "El servicio no fue encontrado.";
                return RedirectToAction("Index");
            }

            return View(response.Result);
        }

        // POST: Servicio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Servicios_Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _servicioService.DeleteAsync(id);
            if (response.IsSuccess)
            {
                TempData["MensajeExito"] = "Servicio eliminado exitosamente.";
            }
            else
            {
                TempData["MensajeError"] = response.Message;
            }

            return RedirectToAction("Index");
        }

    }

}
