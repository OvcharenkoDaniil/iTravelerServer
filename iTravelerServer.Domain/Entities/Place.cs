using System.ComponentModel.DataAnnotations;

namespace iTravelerServer.Domain.Entities;

public class Place
{
    [Key]
    public int Place_id { get; set; }
    public string PlaceName { get; set; }
    public string AirportCity { get; set; }
    public string Tags { get; set; }
    public string Description { get; set; }
}