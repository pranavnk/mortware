using Todo.Api.Requests;

namespace Todo.Api.Handlers;

public class CompleteTodoItemHandler : IRequestHandler<CompleteTodoItemRequest, Guid>
{
    private readonly ITodoRepository _todoRepository;

    public CompleteTodoItemHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<Guid> Handle(CompleteTodoItemRequest request, CancellationToken cancellationToken)
    {
        var itemId = await _todoRepository.Complete(request.Id);
        return itemId;
    }
}