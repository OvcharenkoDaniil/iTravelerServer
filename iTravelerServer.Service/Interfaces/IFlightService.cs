using System.Collections.Generic;
using System.Threading.Tasks;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Response;
using iTravelerServer.Domain.ViewModels.FlightVM;
using iTravelerServer.Domain.ViewModels.OrderVM;


namespace iTravelerServer.Service.Interfaces
{
    public interface IFlightService
    {
        Task<BaseResponse<IEnumerable<Flight>>> GetFlights();
        BaseResponse<List<TicketListVM>> GetTicketList(List<FlightListVM> flightList, SearchRequest filter);
        BaseResponse<List<TicketListVM>> GetOneSideTicketList(List<FlightListVM> flightList, SearchRequest filter);
        BaseResponse<List<FlightListVM>> GetFlightList();

        TicketListVM fillFwTicket(FlightListVM flightElem, TicketListVM ticket, int NumberOfPassangers);
        TicketListVM fillBwTicket(FlightListVM flightElem, TicketListVM ticket);

        bool IsPriceSet(FlightListVM item, string FlightClass, int NumberOfPassangers);
        //BaseResponse<bool> UpdateFlight(int flightId, Flight flight);
        BaseResponse<Flight> GetFlight(int flightId);
        // Task<BaseResponse<Flight>> AddFlight(Flight flightData);
        
        List<int> GetFreePlaces(int bwFlightId, string FlightClass, List<int> listOfUsedValues);
        string FreePlacesToString(List<int> listOfFreevalues);
        List<Flight> FlightGenerator(int numberOfDaysFromToday,int numFlightFromOneCity);

    }   
}