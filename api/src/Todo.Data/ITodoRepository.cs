using Todo.Api.Requests;

namespace Todo.Data;

public interface ITodoRepository
{
    Task<IEnumerable<TodoItem>> List(bool showCompletedItems);

    Task<Guid> Create(TodoItem newItem);

    Task<Guid> Complete(Guid id);
}