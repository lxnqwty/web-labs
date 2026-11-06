using System.ComponentModel.DataAnnotations;

namespace backend.Dtos;

public sealed class CreateTicketRequest
{
    [Required]
    [StringLength(80, MinimumLength = 5)]
    public string Title { get; init; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int CategoryId { get; init; }

    [Required]
    [StringLength(500, MinimumLength = 10)]
    public string Description { get; init; } = string.Empty;
}
