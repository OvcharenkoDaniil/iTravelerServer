using System.Collections.Generic;
using System.Threading.Tasks;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Response;
using iTravelerServer.Domain.ViewModels.FlightVM;


namespace iTravelerServer.Service.Interfaces
{
    public interface IFlightService
    {
        Task<BaseResponse<IEnumerable<Flight>>> GetFlights();
        BaseResponse<List<FlightListVM>> GetFlightList();
        BaseResponse<bool> UpdateFlight(int flightId, Flight flight);
        BaseResponse<Flight> GetFlight(int flightId);
        Task<BaseResponse<Flight>> AddFlight(Flight flightData);
    }   
}