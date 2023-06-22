namespace iTravelerServer.Domain.Entities;
using System.ComponentModel.DataAnnotations;

public class FlightDetails
{
    [Key]
    public int FlightDetail_id { get; set; }
    public int BaggageСompartmentsLeft { get; set; }
    public int BaggageSurchargePrice { get; set; }
    public int ExtraBaggagePrice { get; set; }
    public int MaxExtraBaggagePlacesForPerson { get; set; }
    // public int FirstClassSeatPrice { get; set; }
    public int BusinessClassSeatPrice { get; set; }
    public int StandardSeatPrice { get; set; }
    // public int FirstClassSeatLeft { get; set; }
    public int BusinessClassSeatLeft { get; set; }
    public int StandardSeatLeft { get; set; }
    public int StandardBaggagePlacesForPerson { get; set; }
    public int BusinessBaggagePlacesForPerson { get; set; }
    public int StandardFacility_id { get; set; }
    public int BusinessFacility_id { get; set; }
    // public int FirstClassBaggagePlacesForPerson { get; set; }

}