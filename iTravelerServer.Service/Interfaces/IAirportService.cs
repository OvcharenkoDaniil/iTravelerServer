using System.Collections.Generic;
using System.Threading.Tasks;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Response;


namespace iTravelerServer.Service.Interfaces
{
    public interface IAirportService
    {
        Task<BaseResponse<IEnumerable<Airport>>> GetAirports();
    }   
}