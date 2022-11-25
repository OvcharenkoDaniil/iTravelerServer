using System.Collections.Generic;
using System.Threading.Tasks;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Response;
using iTravelerServer.Domain.ViewModels.FlightVM;


namespace iTravelerServer.Service.Interfaces
{
    public interface ITicketListService
    {
        BaseResponse<List<TicketListVM>> GetFlightList();
        //BaseResponse<List<TicketListVM>> GetTicketList(List<TicketListVM> responseData,TicketSearchRequest filter);

    }   
}