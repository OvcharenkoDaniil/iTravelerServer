using System.ComponentModel.DataAnnotations;

namespace iTravelerServer.Domain.Entities;

public class Baggage
{
    [Key]
    public int Baggage_id { get; set; }
    public int MaxBaggageWeight { get; set; }
    public int StandardBaggageWeight { get; set; }
    public int BusinessBaggageWeight { get; set; }
    // public int FirstClassBaggageWeight { get; set; }
    public int StandardhandLuggageWeight { get; set; }
    public int BusinesshandLuggageWeight { get; set; }
    // public int FirstClasshandLuggageWeight { get; set; }
    
}