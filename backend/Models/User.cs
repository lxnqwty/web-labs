namespace backend.Models;

public sealed class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public ICollection<Ticket> CreatedActivities { get; set; } = new List<Ticket>();
    public ICollection<Ticket> AssignedActivities { get; set; } = new List<Ticket>();
}
