using System.ComponentModel.DataAnnotations;

namespace iTravelerServer.Domain.Entities;

public class Airport
{
    [Key]
    public int Airport_id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    
}