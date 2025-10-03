using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HankoSpa.Services.Interfaces;
using HankoSpa.DTOs;
using HankoSpa.Models;
using System.Security.Claims;

namespace HankoSpa.Controllers
{
    [Route("Citas")]
    [Authorize]
    public class CitasController : Controller
    {
        private readonly ICitaServices _citaService;
        private readonly IServicioServices _servicioService;
        private readonly IUserService _userService;

        public CitasController(ICitaServices citaService, IServicioServices servicioService, IUserService userService)
        {
            _citaService = citaService;
            _servicioService = servicioService;
            _userService = userService;
        }

        // GET: Citas
        [HttpGet("")]
        [Authorize(Policy = "Citas_Read")]
        public async Task<IActionResult> Index()
        {
            var response = await _citaService.GetAllAsync();

            if (!response.IsSuccess)
            {
                ViewBag.ErrorMessage = response.Message;
                return View(new List<Cita>());
            }

            // Obtener el CustomRolId y el UserId del usuario autenticado
            var customRolId = User.FindFirst("CustomRolId")?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            List<Cita> citas;

            // Si es cliente (CustomRolId == 3), solo mostrar sus citas
            if (customRolId == "3")
            {
                citas = response.Result
                    .Where(dto => dto.UsuarioID == userId)
                    .Select(dto => new Cita
                    {
                        CitaId = dto.CitaId,
                        FechaCita = dto.FechaCita,
                        HoraCita = dto.HoraCita,
                        EstadoCita = dto.EstadoCita,
                        UsuarioID = dto.UsuarioID,
                        ServicioId = dto.ServicioID
                    }).ToList();
            }
            else
            {
                // Otros roles ven todas las citas
                citas = response.Result.Select(dto => new Cita
                {
                    CitaId = dto.CitaId,
                    FechaCita = dto.FechaCita,
                    HoraCita = dto.HoraCita,
                    EstadoCita = dto.EstadoCita,
                    UsuarioID = dto.UsuarioID,
                    ServicioId = dto.ServicioID
                }).ToList();
            }

            return View(citas);
        }

        // GET: Citas/Create
        [HttpGet("Create")]
        [Authorize(Policy = "Citas_Create")]
        public async Task<IActionResult> Create()
        {
            await CargarUsersAsync();
            await CargarServiciosAsync();
            return View();
        }

        // GET: Citas/Details/5
        [HttpGet("Details/{id}")]
        [Authorize(Policy = "Citas_Read")]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _citaService.GetOneAsync(id);
            if (!response.IsSuccess || response.Result == null)
            {
                TempData["MensajeError"] = "La cita no fue encontrada.";
                return RedirectToAction("Index");
            }

            // Obtener el documento del usuario
            string documentoUsuario = "";
            if (Guid.TryParse(response.Result.UsuarioID, out Guid usuarioGuid))
            {
                var usuario = await _userService.GetUserByIdAsync(usuarioGuid);
                documentoUsuario = usuario?.Document ?? "";
            }
            ViewBag.DocumentoUsuario = documentoUsuario;

            return View(response.Result);
        }

        // POST: Citas/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Citas_Create")]
        public async Task<IActionResult> Create(CitaDTO citaDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _citaService.CreateAsync(citaDTO);

                if (response.IsSuccess)
                {
                    TempData["MensajeExito"] = "Cita creada exitosamente.";
                    return RedirectToAction("Index");
                }

                ViewBag.ErrorMessage = response.Message;
            }

            await CargarServiciosAsync(citaDTO.ServicioID);
            await CargarUsersAsync(citaDTO.UsuarioID);
            return View(citaDTO);
        }

        // GET: Citas/Edit/5
        [HttpGet("Edit/{id}")]
        [Authorize(Policy = "Citas_Update")]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _citaService.GetOneAsync(id);
            if (!response.IsSuccess || response.Result == null)
            {
                TempData["MensajeError"] = "La cita no fue encontrada.";
                return RedirectToAction("Index");
            }

            await CargarServiciosAsync(response.Result.ServicioID);
            await CargarUsersAsync(response.Result.UsuarioID);
            return View(response.Result);
        }

        // POST: Citas/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Citas_Update")]
        public async Task<IActionResult> Edit(int id, CitaDTO citaDTO)
        {
            if (id != citaDTO.CitaId) return NotFound();

            if (ModelState.IsValid)
            {
                // Forzar el estado a "Agendada" al editar
                citaDTO.EstadoCita = "Agendada";

                // El UsuarioID ya viene del formulario, asegúrate de que el select esté en la vista
                var response = await _citaService.EditAsync(citaDTO);
                if (response.IsSuccess)
                {
                    TempData["MensajeExito"] = "Cita actualizada exitosamente.";
                    return RedirectToAction("Index");
                }

                ViewBag.ErrorMessage = response.Message;
            }

            await CargarServiciosAsync(citaDTO.ServicioID);
            await CargarUsersAsync(citaDTO.UsuarioID); // Asegura que el select de usuario se rellene correctamente
            return View(citaDTO);
        }

        // GET: Citas/Delete/5
        [HttpGet("Delete/{id}")]
        [Authorize(Policy = "Citas_Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var response = await _citaService.GetOneAsync(id.Value);
            if (!response.IsSuccess || response.Result == null)
            {
                TempData["MensajeError"] = "La cita no fue encontrada.";
                return RedirectToAction("Index");
            }

            return View(response.Result);
        }

        // POST: Citas/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Citas_Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _citaService.DeleteAsync(id);
            if (response.IsSuccess)
            {
                TempData["MensajeExito"] = "Cita eliminada exitosamente.";
            }
            else
            {
                TempData["MensajeError"] = response.Message;
            }

            return RedirectToAction("Index");
        }

        // Método auxiliar para cargar servicios
        private async Task CargarServiciosAsync(int? seleccionado = null)
        {
            var servicios = await _servicioService.GetAllAsync();
            ViewBag.Servicios = new SelectList(servicios.Result, "ServicioId", "NombreServicio", seleccionado);
        }

        // Método auxiliar para cargar Users
        private async Task CargarUsersAsync(string? seleccionado = null)
        {
            var users = await _userService.GetAllUsersComboAsync();
            ViewBag.Users = new SelectList(users.Result, "Id", "FullName", seleccionado);
        }
    }
}



