using System.ComponentModel.DataAnnotations;

namespace iTravelerServer.Domain.Entities;

public class Order
{
    [Key]
    public int Order_id { get; set; }
    public int User_id { get; set; }
    public int NumberOfTickets { get; set; }
    public int Ticket_id { get; set; }
    
    public string FlightClass { get; set; }
    
    public DateTime CreationDate { get; set; }    
    public DateTime ExpirationDate { get; set; }    
}