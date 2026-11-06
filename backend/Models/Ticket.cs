namespace backend.Models;

public sealed record Ticket(
    int Id,
    int Number,
    string Title,
    int CategoryId,
    string Description,
    string Status,
    int? AssigneeUserId);
