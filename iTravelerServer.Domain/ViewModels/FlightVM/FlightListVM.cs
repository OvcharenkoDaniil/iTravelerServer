namespace iTravelerServer.Domain.ViewModels.FlightVM;

public class FlightListVM
{
    public int FwPrice { get; set; }
    public int Flight_id { get; set; }

    public string FwAircompany_name { get; set; }
    public int FwStandardClassCapacity { get; set; }
    public int FwFirstClassCapacity { get; set; }
    public string FwDepartureCity { get; set; }
    public string FwDepartureAirportName { get; set; }
    public string FwArrivalAirportName { get; set; }
    public string FwDepartureAirportCountry { get; set; }
    public string FwArrivalAirportCountry { get; set; }

    public string FwArrivalCity { get; set; }
    public DateTime FwDepartureDate { get; set; }
    public DateTime FwArrivalDate { get; set; }
    public string FwDepartureTime { get; set; }
    public string FwArrivalTime { get; set; }
    public string FwFlightDuration { get; set; }
    public int FwNumberOfTransfers { get; set; }
    

    
}