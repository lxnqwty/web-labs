using backend.Dtos;
using backend.Models;

namespace backend.Services;

public sealed class TicketService
{
    private readonly object _sync = new();
    private readonly List<Ticket> _tickets = [];
    private int _nextId = 1;
    private int _nextNumber = 1001;

    public IReadOnlyList<TicketResponse> GetAll()
    {
        lock (_sync)
        {
            return _tickets.Select(ToResponse).ToArray();
        }
    }

    public TicketResponse? GetById(int id)
    {
        lock (_sync)
        {
            var ticket = _tickets.FirstOrDefault(item => item.Id == id);
            return ticket is null ? null : ToResponse(ticket);
        }
    }

    public TicketResponse Create(CreateTicketRequest request)
    {
        lock (_sync)
        {
            var ticket = new Ticket(
                Id: _nextId++,
                Number: _nextNumber++,
                Title: request.Title.Trim(),
                CategoryId: request.CategoryId,
                Description: request.Description.Trim(),
                Status: "New",
                AssigneeUserId: null);

            _tickets.Insert(0, ticket);
            return ToResponse(ticket);
        }
    }

    public TicketResponse? Assign(int id, int assigneeUserId)
    {
        lock (_sync)
        {
            var index = _tickets.FindIndex(item => item.Id == id);
            if (index < 0) return null;

            var updated = _tickets[index] with { AssigneeUserId = assigneeUserId };
            _tickets[index] = updated;
            return ToResponse(updated);
        }
    }

    public bool TryChangeStatus(
        int id,
        string status,
        int? currentUserId,
        out TicketResponse? response,
        out string? error)
    {
        lock (_sync)
        {
            var index = _tickets.FindIndex(item => item.Id == id);
            if (index < 0)
            {
                response = null;
                error = "Заявка не найдена.";
                return false;
            }

            var ticket = _tickets[index];
            if (status == "InProgress" || status == "Closed")
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

            var updated = ticket with { Status = status };
            _tickets[index] = updated;
            response = ToResponse(updated);
            error = null;
            return true;
        }
    }

    private static TicketResponse ToResponse(Ticket ticket) => new(
        ticket.Id,
        ticket.Number,
        ticket.Title,
        ticket.CategoryId,
        ticket.Description,
        ticket.Status,
        ticket.AssigneeUserId);
}
