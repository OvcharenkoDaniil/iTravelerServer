using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Response;
using iTravelerServer.Domain.ViewModels.FlightVM;

namespace iTravelerServer.Service.Interfaces;


    public interface ITicketService
    {
        Task<BaseResponse<IEnumerable<Ticket>>> GetTickets();
        BaseResponse<List<TicketListVM>> GetTicketList(List<FlightListVM> flightList, TicketSearchRequest filter);
        BaseResponse<Ticket> AddTicket(Ticket ticket);

        TicketListVM createTicket(
            FlightListVM fwTicket,
            FlightListVM bwTicket,
            int NumberOfPassangers,
            int i
        );

    }   
