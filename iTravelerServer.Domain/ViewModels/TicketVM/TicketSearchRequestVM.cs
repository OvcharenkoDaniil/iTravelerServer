namespace iTravelerServer.Domain.Entities;

public class SearchRequest
{
    public DateTime DepartureDate { get; set; }
    
    public DateTime ReturnDate { get; set; }
        
    public string DepartureCity { get; set; }
        
    public string ArrivalCity { get; set; }
    
    public int NumberOfPassangers { get; set; }
    public string FlightClass { get; set; }
    public bool IsOneSide { get; set; }
    
}