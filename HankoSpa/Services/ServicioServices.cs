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

namespace HankoSpa.Services
{
    public class ServicioService : IServicioService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ServicioService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<List<ServiceDTO>>> GetAllAsync()
        {
            var response = new Response<List<ServiceDTO>>();
            try
            {
                var servicios = await _context.Servicios.ToListAsync();
                var dtoList = _mapper.Map<List<ServiceDTO>>(servicios);
                response.IsSuccess = true;
                response.Result = dtoList;
                response.Message = "Servicios obtenidos correctamente.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new List<string> { ex.Message };
                response.Message = "Error al obtener los servicios.";
            }

            return response;
        }

        public async Task<Response<ServiceDTO>> GetOneAsync(int id)
        {
            var response = new Response<ServiceDTO>();
            try
            {
                var servicio = await _context.Servicios.FindAsync(id);
                if (servicio == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Servicio no encontrado.";
                    return response;
                }

                response.IsSuccess = true;
                response.Result = _mapper.Map<ServiceDTO>(servicio);
                response.Message = "Servicio obtenido correctamente.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new List<string> { ex.Message };
                response.Message = "Error al obtener el servicio.";
            }

            return response;
        }

        public async Task<Response<ServiceDTO>> CreateAsync(ServiceDTO dto)
        {
            var response = new Response<ServiceDTO>();
            try
            {
                var servicio = _mapper.Map<Servicio>(dto);
                _context.Servicios.Add(servicio);
                await _context.SaveChangesAsync();

                response.IsSuccess = true;
                response.Result = _mapper.Map<ServiceDTO>(servicio);
                response.Message = "Servicio creado correctamente.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new List<string> { ex.Message };
                response.Message = "Error al crear el servicio.";
            }

            return response;
        }

        public async Task<Response<ServiceDTO>> EditAsync(ServiceDTO dto)
        {
            var response = new Response<ServiceDTO>();
            try
            {
                var servicio = await _context.Servicios.FindAsync(dto.ServicioId);
                if (servicio == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Servicio no encontrado.";
                    return response;
                }

                // Actualiza propiedades
                servicio.NombreServicio = dto.NombreServicio;
                servicio.DescripcionServicio = dto.DescripcionServicio;

                _context.Servicios.Update(servicio);
                await _context.SaveChangesAsync();

                response.IsSuccess = true;
                response.Result = _mapper.Map<ServiceDTO>(servicio);
                response.Message = "Servicio actualizado correctamente.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new List<string> { ex.Message };
                response.Message = "Error al editar el servicio.";
            }

            return response;
        }

        public async Task<Response<object>> DeleteAsync(int id)
        {
            var response = new Response<object>();
            try
            {
                var servicio = await _context.Servicios.FindAsync(id);
                if (servicio == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Servicio no encontrado.";
                    return response;
                }

                _context.Servicios.Remove(servicio);
                await _context.SaveChangesAsync();

                response.IsSuccess = true;
                response.Message = "Servicio eliminado correctamente.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new List<string> { ex.Message };
                response.Message = "Error al eliminar el servicio.";
            }

            return response;
        }
    }
}