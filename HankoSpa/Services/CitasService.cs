/*using AutoMapper;
using HankoSpa.Data;
using HankoSpa.DTOs;
using HankoSpa.Models;
using HankoSpa.Nucleo;
using HankoSpa.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HankoSpa.Services
{
    public class CitasService : ICitasService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CitasService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<List<CitasDTO>>> GetAllAsync()
        {
            var response = new Response<List<CitasDTO>>();
            try
            {
                var citas = await _context.Citas.ToListAsync();
                var dtoList = _mapper.Map<List<CitasDTO>>(citas);
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

        public async Task<Response<CitasDTO>> GetOneAsync(int id)
        {
            var response = new Response<CitasDTO>();
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
                response.Result = _mapper.Map<CitasDTO>(cita);
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

        public async Task<Response<CitasDTO>> CreateAsync(CitasDTO dto)
        {
            var response = new Response<CitasDTO>();
            try
            {
                var cita = _mapper.Map<Cita>(dto);
                _context.Citas.Add(cita);
                await _context.SaveChangesAsync();

                response.IsSuccess = true;
                response.Result = _mapper.Map<CitasDTO>(cita);
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

        public async Task<Response<CitasDTO>> EditAsync(CitasDTO dto)
        {
            var response = new Response<CitasDTO>();
            try
            {
                var cita = await _context.Citas.FindAsync(dto.CitasID);
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

                response.IsSuccess = true;
                response.Result = _mapper.Map<CitasDTO>(cita);
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
*/