namespace iTravelerServer.Domain.ViewModels.TicketVM;

public class TicketVM
{
    public int Ticket_id { get; set; }
    public int Price { get; set; }
    public string FlightClass { get; set; }

    public int FwFlight_id { get; set; }
    public int BwFlight_id { get; set; }
    public int FwFirst_ticket_num { get; set; }
    public int FwSecond_ticket_num { get; set; }
    public int FwThird_ticket_num { get; set; }
    public int BwFirst_ticket_num { get; set; }
    public int BwSecond_ticket_num { get; set; }
    public int BwThird_ticket_num { get; set; }
}