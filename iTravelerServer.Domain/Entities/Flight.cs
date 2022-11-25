using System.ComponentModel.DataAnnotations;

namespace iTravelerServer.Domain.Entities;

public class Flight
{
    [Key]
    public int Flight_id { get; set; }
    public int Plane_id { get; set; }
    
    public DateTime DepartureDate { get; set; }
    public DateTime ArrivalDate { get; set; }
        
    public string DepartureTime { get; set; }
    public string ArrivalTime { get; set; }

    public string FlightDuration { get; set; }
    public int Transfer_id { get; set; }
    public int DepartureAirport_id { get; set; }
    public int ArrivalAirport_id { get; set; }
    
    public int Price { get; set; }

        

}