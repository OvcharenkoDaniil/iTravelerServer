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
    public class PlaneService : IPlaneService
    {
        private readonly IBaseRepository<Plane> _planeRepository;

        public PlaneService(IBaseRepository<Plane> planeRepository)
        {
            _planeRepository = planeRepository;
        }

        public async Task<BaseResponse<IEnumerable<Plane>>> GetPlanes()
        {
            var baseResponse = new BaseResponse<IEnumerable<Plane>>();
            try
            {
                var planes = await _planeRepository.GetAll().ToListAsync();

                return new BaseResponse<IEnumerable<Plane>>()
                {
                    Data = planes,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Plane>>()
                {
                    Description = $"[GetPlanes] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }

        }
    }   
}