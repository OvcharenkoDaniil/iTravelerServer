using System.ComponentModel.DataAnnotations;

namespace iTravelerServer.Domain.Entities;

public class Plane
{
    [Key]
    public int Plane_id { get; set; }
    public int FirstClassCapacity { get; set; }
    public int StandardClassCapacity { get; set; }
    
    public string Name { get; set; }
    public string Type { get; set; }
    public string Aircompany_name { get; set; }
    
}