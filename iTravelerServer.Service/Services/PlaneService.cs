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
                
                    // .Select(x => new CarViewModel()
                    // {
                    //     Id = x.Id,
                    //     Speed = x.Speed,
                    //     Name = x.Name,
                    //     Description = x.Description,
                    //     Model = x.Model,
                    //     DateCreate = x.DateCreate.ToLongDateString(),
                    //     Price = x.Price,
                    //     TypeCar = x.TypeCar.GetDisplayName()
                    // })
                    // .Where(x => EF.Functions.Like(x.Name, $"%{term}%"))
                    // .ToDictionaryAsync(x => x.Id, t => t.Name);

                baseResponse.Data = planes;
                return baseResponse;
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