using System.ComponentModel.DataAnnotations;

namespace iTravelerServer.Domain.Entities;

public class Order
{
    [Key]
    public int Order_id { get; set; }
    public int Account_id { get; set; }
    public int TotalPrice { get; set; }
    public int NumberOfTickets { get; set; }
    public int NumberOfFlights { get; set; }
    public int FwFlight_id { get; set; }
    public int BwFlight_id { get; set; }
    public int FwOrderDetail_id { get; set; }
    public int BwOrderDetail_id { get; set; }
    public DateTime CreationDate { get; set; }    
    public DateTime ExpirationDate { get; set; }    
}