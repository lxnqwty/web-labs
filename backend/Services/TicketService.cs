using backend.Data;
using backend.Dtos;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public sealed class TicketService(AppDbContext db, IConfiguration configuration)
{
    private readonly int _defaultCreatedByUserId =
        configuration.GetValue<int?>("Tickets:DefaultCreatedByUserId") ?? 1;

    public IReadOnlyList<TicketResponse> GetAll() =>
        db.Activities.AsNoTracking()
            .OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id)
            .ToList()
            .Select(ToResponse)
            .ToArray();

    public TicketResponse? GetById(int id)
    {
        var ticket = db.Activities.AsNoTracking().FirstOrDefault(x => x.Id == id);
        return ticket is null ? null : ToResponse(ticket);
    }

    public TicketResponse Create(CreateTicketRequest request)
    {
        var ticket = new Ticket
        {
            Title = request.Title.Trim(),
            CategoryId = request.CategoryId,
            Description = request.Description.Trim(),
            Status = "New",
            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = _defaultCreatedByUserId
        };

        db.Activities.Add(ticket);
        db.SaveChanges();
        return ToResponse(ticket);
    }

    public TicketResponse? Assign(int id, int assigneeUserId)
    {
        var ticket = db.Activities.FirstOrDefault(x => x.Id == id);
        if (ticket is null) return null;

        ticket.AssigneeUserId = assigneeUserId;
        db.SaveChanges();
        return ToResponse(ticket);
    }

    public bool TryChangeStatus(
        int id,
        string status,
        int? currentUserId,
        out TicketResponse? response,
        out string? error)
    {
        var ticket = db.Activities.FirstOrDefault(x => x.Id == id);
        if (ticket is null)
        {
            response = null;
            error = "Заявка не найдена.";
            return false;
        }

        if (status is "InProgress" or "Closed")
        {
            if (ticket.AssigneeUserId is null)
            {
                response = null;
                error = "Сначала назначьте исполнителя.";
                return false;
            }

            if (currentUserId != ticket.AssigneeUserId)
            {
                response = null;
                error = "Изменить статус может только назначенный исполнитель (X-User-Id).";
                return false;
            }
        }

        if (status == "InProgress" && ticket.Status != "New")
        {
            response = null;
            error = "В работу можно перевести только новую заявку.";
            return false;
        }

        if (status == "Closed" && ticket.Status != "InProgress")
        {
            response = null;
            error = "Закрыть можно только заявку, находящуюся на проверке.";
            return false;
        }

        if (status == "Cancelled" && ticket.Status == "Closed")
        {
            response = null;
            error = "Подтверждённую заявку нельзя отменить.";
            return false;
        }

        if (status == "New" && ticket.Status != "New")
        {
            response = null;
            error = "Нельзя вернуть заявку в начальный статус.";
            return false;
        }

        ticket.Status = status;
        db.SaveChanges();
        response = ToResponse(ticket);
        error = null;
        return true;
    }

    private static TicketResponse ToResponse(Ticket ticket) => new(
        ticket.Id, ticket.Number, ticket.Title, ticket.CategoryId,
        ticket.Description, ticket.Status, ticket.AssigneeUserId);
}
