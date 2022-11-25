using System.ComponentModel.DataAnnotations;

namespace iTravelerServer.Domain.Entities;

public class Ticket
{
    [Key]
    public int Ticket_id { get; set; }
    public int Price { get; set; }
    public int FwFlight_id { get; set; }
    public int BwFlight_id { get; set; }

}