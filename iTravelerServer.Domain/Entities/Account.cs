using System.ComponentModel.DataAnnotations;
using iTravelerServer.Domain.Enum;

namespace iTravelerServer.Domain.Entities;

public class Account
{
    [Key]
    public int Account_id { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public string Username { get; set; }
    
    public Role Role { get; set; }
    
    
}