using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Response;
using iTravelerServer.Domain.ViewModels.FlightVM;
using iTravelerServer.Domain.ViewModels.TicketVM;

namespace iTravelerServer.Service.Interfaces;


    public interface ITicketService
    {
        Task<BaseResponse<IEnumerable<Ticket>>> GetTickets();
        BaseResponse<List<TicketListVM>> GetTicketList(List<FlightListVM> flightList, TicketSearchRequest filter);
        BaseResponse<Ticket> AddTicket(TicketVM ticketVm);
        //public List<int> GetFreeFwPlaces(int fwFlightId, string FlightClass);
        //public List<int> GetFreeBwPlaces(int bwFlightId, string FlightClass);
        public List<int> GetFreePlaces(int bwFlightId, string FlightClass, List<TicketDetail> ticketDetail);
        public List<TicketDetail> GetFwTicketDetail(int FlightId);
        public List<TicketDetail> GetBwTicketDetail(int FlightId);
        TicketListVM createTicket(
            FlightListVM fwTicket,
            FlightListVM bwTicket,
            int NumberOfPassangers,
            int i
        );

    }   
