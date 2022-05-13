using Microsoft.AspNetCore.Mvc;
using Todo.Api.Requests;

namespace Todo.Api.Controllers;

[ApiController]
public class TodoController : ControllerBase
{
    private readonly IMediator _mediator;

    public TodoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("todo/list/{showCompletedItems}")]
    public async Task<IActionResult> List([FromRoute]bool showCompletedItems) =>
        Ok(await _mediator.Send(new ListTodoItemsRequest { ShowCompletedItems = showCompletedItems}));

    [HttpPost("todo/create")]
    public async Task<IActionResult> Get([FromBody] CreateTodoItemRequest request) =>
        Ok(await _mediator.Send(request));

    [HttpPatch("todo/complete")]
    public async Task<IActionResult> Patch([FromBody] CompleteTodoItemRequest request) =>
        Ok(await _mediator.Send(request));
}