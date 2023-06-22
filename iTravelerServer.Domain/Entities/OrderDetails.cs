using System.ComponentModel.DataAnnotations;

namespace iTravelerServer.Domain.Entities;

public class OrderDetails
{
    [Key]
    public int Order_Detail_id { get; set; }
    public int Order_id { get; set; }
    public int NumberOfBaggagePlaces { get; set; }
    public int TotalExtraBaggageWeight { get; set; }
    public int Price { get; set; }
    public string Direction { get; set; }
    public string SeatNumbers { get; set; }
    public string FlightClass { get; set; }
    
    
    
}