using backend.Dtos;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/tickets")]
public sealed class TicketsController(
    TicketService ticketService,
    ILogger<TicketsController> logger) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            return Ok(ticketService.GetAll());
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "GetTickets failed; error={ErrorType}", exception.GetType().Name);
            return Problem(statusCode: 500, title: "Не удалось обработать запрос.");
        }
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        try
        {
            var ticket = ticketService.GetById(id);
            return ticket is null
                ? Problem(statusCode: 404, title: "Заявка не найдена.")
                : Ok(ticket);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "GetTicket failed; ticketId={TicketId}; error={ErrorType}", id, exception.GetType().Name);
            return Problem(statusCode: 500, title: "Не удалось обработать запрос.");
        }
    }

    [HttpPost]
    public IActionResult Create(CreateTicketRequest request)
    {
        try
        {
            var ticket = ticketService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, ticket);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "CreateTicket failed; error={ErrorType}", exception.GetType().Name);
            return Problem(statusCode: 500, title: "Не удалось обработать запрос.");
        }
    }

    [HttpPatch("{id:int}/assignee")]
    public IActionResult Assign(int id, AssignTicketRequest request)
    {
        try
        {
            var ticket = ticketService.Assign(id, request.AssigneeUserId);
            return ticket is null
                ? Problem(statusCode: 404, title: "Заявка не найдена.")
                : Ok(ticket);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "AssignTicket failed; ticketId={TicketId}; error={ErrorType}", id, exception.GetType().Name);
            return Problem(statusCode: 500, title: "Не удалось обработать запрос.");
        }
    }

    [HttpPatch("{id:int}/status")]
    public IActionResult ChangeStatus(int id, ChangeTicketStatusRequest request)
    {
        try
        {
            if (ticketService.GetById(id) is null)
                return Problem(statusCode: 404, title: "Заявка не найдена.");

            int? currentUserId = int.TryParse(Request.Headers["X-User-Id"], out var parsedUserId)
                ? parsedUserId
                : null;

            var succeeded = ticketService.TryChangeStatus(
                id, request.Status, currentUserId, out var ticket, out var error);

            return succeeded
                ? Ok(ticket)
                : Problem(statusCode: 409, title: "Конфликт состояния заявки.", detail: error);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "ChangeTicketStatus failed; ticketId={TicketId}; error={ErrorType}", id, exception.GetType().Name);
            return Problem(statusCode: 500, title: "Не удалось обработать запрос.");
        }
    }
}
