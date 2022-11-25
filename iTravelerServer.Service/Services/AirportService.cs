using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iTravelerServer.DAL.Interfaces;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Enum;
using iTravelerServer.Domain.Response;
using iTravelerServer.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iTravelerServer.Service.Services
{
    public class AirportService : IAirportService
    {
        private readonly IBaseRepository<Airport> _airportRepository;

        public AirportService(IBaseRepository<Airport> airportRepository)
        {
            _airportRepository = airportRepository;
        }

        public async Task<BaseResponse<IEnumerable<Airport>>> GetAirports()
        {
            var baseResponse = new BaseResponse<IEnumerable<Airport>>();
            try
            {
                var airports = await _airportRepository.GetAll().ToListAsync();
                
                

                baseResponse.Data = airports;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Airport>>()
                {
                    Description = $"[GetAirports] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }

        }
    }   
}