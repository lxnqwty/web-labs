using System.ComponentModel.DataAnnotations;

namespace backend.Dtos;

public sealed class ChangeTicketStatusRequest
{
    [Required]
    [RegularExpression("^(New|InProgress|Closed|Cancelled)$")]
    public string Status { get; init; } = string.Empty;
}
