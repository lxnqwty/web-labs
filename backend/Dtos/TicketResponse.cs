namespace backend.Dtos;

public sealed record TicketResponse(
    int Id,
    int Number,
    string Title,
    int CategoryId,
    string Description,
    string Status,
    int? AssigneeUserId);
