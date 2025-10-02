using AutoMapper;
using HankoSpa.Data;
using HankoSpa.DTOs;
using HankoSpa.Models;
using HankoSpa.Nucleo;
using HankoSpa.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HankoSpa.Services; // Agregado para usar SimpleCacheManager

namespace HankoSpa.Services
{
    public class CitasService : ICitaServices
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private const string CacheKey = "Citas";

        public CitasService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<List<CitaDTO>>> GetAllAsync()
        {
            // Intentar obtener de caché
            var cached = SimpleCacheManager.Instance.Get<List<CitaDTO>>(CacheKey);
            if (cached != null)
            {
                return new Response<List<CitaDTO>>(true, "OK (desde caché)", cached);
            }

            var response = new Response<List<CitaDTO>>();
            try
            {
                var citas = await _context.Citas.ToListAsync();
                var dtoList = _mapper.Map<List<CitaDTO>>(citas);

                // Guardar en caché
                SimpleCacheManager.Instance.Set(CacheKey, dtoList);

                response.IsSuccess = true;
                response.Result = dtoList;
                response.Message = "Citas obtenidas correctamente.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new List<string> { ex.Message };
                response.Message = "Error al obtener las citas.";
            }

            return response;
        }

        public async Task<Response<CitaDTO>> GetOneAsync(int id)
        {
            var response = new Response<CitaDTO>();
            try
            {
                var cita = await _context.Citas.FindAsync(id);
                if (cita == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Cita no encontrada.";
                    return response;
                }

                response.IsSuccess = true;
                response.Result = _mapper.Map<CitaDTO>(cita);
                response.Message = "Cita obtenida correctamente.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new List<string> { ex.Message };
                response.Message = "Error al obtener la cita.";
            }

            return response;
        }

        public async Task<Response<CitaDTO>> CreateAsync(CitaDTO dto)
        {
            var response = new Response<CitaDTO>();
            try
            {
                var cita = _mapper.Map<Cita>(dto);
                _context.Citas.Add(cita);
                await _context.SaveChangesAsync();

                // Limpiar caché porque los datos han cambiado
                SimpleCacheManager.Instance.Remove(CacheKey);

                response.IsSuccess = true;
                response.Result = _mapper.Map<CitaDTO>(cita);
                response.Message = "Cita creada correctamente.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new List<string> { ex.Message };
                response.Message = "Error al crear la cita.";
            }

            return response;
        }

        public async Task<Response<CitaDTO>> EditAsync(CitaDTO dto)
        {
            var response = new Response<CitaDTO>();
            try
            {
                var cita = await _context.Citas.FindAsync(dto.CitaId);
                if (cita == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Cita no encontrada.";
                    return response;
                }

                // Actualiza los campos necesarios
                cita.FechaCita = dto.FechaCita;
                cita.HoraCita = dto.HoraCita;
                cita.EstadoCita = dto.EstadoCita;
                cita.UsuarioID = dto.UsuarioID;

                _context.Citas.Update(cita);
                await _context.SaveChangesAsync();

                // Limpiar caché porque los datos han cambiado
                SimpleCacheManager.Instance.Remove(CacheKey);

                response.IsSuccess = true;
                response.Result = _mapper.Map<CitaDTO>(cita);
                response.Message = "Cita actualizada correctamente.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new List<string> { ex.Message };
                response.Message = "Error al editar la cita.";
            }

            return response;
        }

        public async Task<Response<object>> DeleteAsync(int id)
        {
            var response = new Response<object>();
            try
            {
                var cita = await _context.Citas.FindAsync(id);
                if (cita == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Cita no encontrada.";
                    return response;
                }

                _context.Citas.Remove(cita);
                await _context.SaveChangesAsync();

                // Limpiar caché porque los datos han cambiado
                SimpleCacheManager.Instance.Remove(CacheKey);

                response.IsSuccess = true;
                response.Message = "Cita eliminada correctamente.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new List<string> { ex.Message };
                response.Message = "Error al eliminar la cita.";
            }

            return response;
        }
    }
}