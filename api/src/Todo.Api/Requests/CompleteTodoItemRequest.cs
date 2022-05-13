namespace Todo.Api.Requests;

public class CompleteTodoItemRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
}