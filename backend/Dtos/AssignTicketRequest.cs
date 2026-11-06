using System.ComponentModel.DataAnnotations;

namespace backend.Dtos;

public sealed class AssignTicketRequest
{
    [Range(1, int.MaxValue)]
    public int AssigneeUserId { get; init; }
}
