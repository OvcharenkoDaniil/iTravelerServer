using System.ComponentModel.DataAnnotations;

namespace iTravelerServer.Domain.Entities;

public class TicketDetail
{
    [Key]
    public int Ticket_Detail_id { get; set; }
    public int Ticket_id { get; set; }
    public string Direction { get; set; }
    public int First_ticket_num { get; set; }
    public int Second_ticket_num { get; set; }
    public int Third_ticket_num { get; set; }
}