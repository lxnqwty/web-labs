namespace backend.Models;

public sealed class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Ticket> Activities { get; set; } = new List<Ticket>();
}
