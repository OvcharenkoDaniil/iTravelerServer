using System.ComponentModel.DataAnnotations;

namespace iTravelerServer.Domain.Entities;

public class Facility
{
    [Key]
    public int Facility_id { get; set; }
    public string USB { get; set; }
    public string WiFi { get; set; }
    public string Entertainment { get; set; }
    public string Drink { get; set; }
    public string Food { get; set; }
}