namespace iTravelerServer.Domain.ViewModels.FlightVM;

public class FlightListVM
{
    public int Price { get; set; }
    public string SeatNumbers { get; set; }
    public int TotalExtraBaggageWeight { get; set; }
    public int NumberOfBaggagePlaces { get; set; }
    
    //public int FirstClassSeatPrice { get; set; }
    public int MaxBaggageWeight { get; set; }
    public int StandardBaggageWeight { get; set; }
    public int BusinessBaggageWeight { get; set; }
    public int StandardhandLuggageWeight { get; set; }
    public int BusinesshandLuggageWeight { get; set; }
    public int BusinessClassSeatPrice { get; set; }
    public int StandardSeatPrice { get; set; }
    public int StandardClassCapacity { get; set; }
    public int BusinessClassCapacity { get; set; }
    
    
    
    public string StandardUSB { get; set; }
    public string StandardWiFi { get; set; }
    public string StandardEntertainment { get; set; }
    public string StandardDrink { get; set; }
    public string StandardFood { get; set; }
    public string BusinessUSB { get; set; }
    public string BusinessWiFi { get; set; }
    public string BusinessEntertainment { get; set; }
    public string BusinessDrink { get; set; }
    public string BusinessFood { get; set; }
    
    //public int FirstClassCapacity { get; set; }
    public int Flight_id { get; set; }
    public int ExtraBaggagePrice { get; set; }
    public int MaxExtraBaggagePlacesForPerson { get; set; }
    public int BaggageSurchargePrice { get; set; }
    public int StandardBaggagePlacesForPerson { get; set; }
    public int BusinessBaggagePlacesForPerson { get; set; }
    public string Aircompany_name { get; set; }
    public string PlaneName { get; set; }
    public int PlaneId { get; set; }
    public int StandardSeatsInRow { get; set; }
    public int BusinessSeatsInRow { get; set; }
    // public string FlightClass { get; set; }
    public int StandardSeatLeft { get; set; }
    public int BusinessClassSeatLeft { get; set; }
    //public int FirstClassSeatLeft { get; set; }
    public string DepartureCity { get; set; }
    public string DepartureAirportName { get; set; }
    public string ArrivalAirportName { get; set; }
    public string DepartureAirportCountry { get; set; }
    public string ArrivalAirportCountry { get; set; }
    public string ArrivalCity { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ArrivalDate { get; set; }
    public string DepartureTime { get; set; }
    public string ArrivalTime { get; set; }
    public string FlightDuration { get; set; }
    public int NumberOfTransfers { get; set; }
    public string TimeZone { get; set; }
    public string FirstTransferDuration { get; set; }
    public string FirstTransferCity { get; set; }
}