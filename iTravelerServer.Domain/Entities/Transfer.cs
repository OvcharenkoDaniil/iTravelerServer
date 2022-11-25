using System.ComponentModel.DataAnnotations;

namespace iTravelerServer.Domain.Entities;

public class Transfer
{
    [Key]
    public int Transfer_id { get; set; }
    public int NumberOfTransfers { get; set; }
    public string FirstTransferCity { get; set; }
    public string SecondTransferCity { get; set; }
    public string FirstTransferDuration { get; set; }
    public string SecondTransferDuration { get; set; }
}