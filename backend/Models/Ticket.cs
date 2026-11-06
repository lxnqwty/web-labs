namespace backend.Models;

// EF Core entity stored in PostgreSQL table activities.
public sealed class Ticket
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string Title { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "New";
    public DateTime CreatedAt { get; set; }
    public int CreatedByUserId { get; set; }
    public int? AssigneeUserId { get; set; }

    public Category Category { get; set; } = null!;
    public User CreatedByUser { get; set; } = null!;
    public User? AssigneeUser { get; set; }
}
