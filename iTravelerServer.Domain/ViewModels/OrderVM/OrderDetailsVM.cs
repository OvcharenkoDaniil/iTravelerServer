namespace iTravelerServer.Domain.ViewModels.OrderVM;

public class OrderDetailsVM
{
    public int FwFlight_id { get; set; }
    public int BwFlight_id { get; set; }
    public int FwNumberOfBaggagePlaces { get; set; }
    public int BwNumberOfBaggagePlaces { get; set; }
    public int FwPrice { get; set; }
    public int BwPrice { get; set; }
    public int NumberOfTickets { get; set; }
    public string FwSeatNumbers { get; set; }
    public string BwSeatNumbers { get; set; }
    public string FlightClass { get; set; } 
    public bool IsOneSide { get; set; } 
    
    ////////////////////Order
    public string Email { get; set; }
    public int FwTotalExtraBaggageWeight { get; set; }
    public int BwTotalExtraBaggageWeight { get; set; }
}