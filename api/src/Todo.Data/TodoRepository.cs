using Microsoft.EntityFrameworkCore;
using Todo.Api.Requests;

namespace Todo.Data;

public class TodoRepository: ITodoRepository
{
    private readonly TodoContext _context;

    public TodoRepository(TodoContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TodoItem>> List(bool showCompletedItems)
    {
        var itemsQuery = _context
            .TodoItems
            .OrderByDescending(x => x.Created);
        if (showCompletedItems)
        {
            return await itemsQuery
            .ToArrayAsync();
        }
        else
        {
            return await itemsQuery
            .Where(item => !item.Completed.HasValue)
            .ToArrayAsync();
        }
    }

    public async Task<Guid> Create(TodoItem newItem)
    {
        await _context.TodoItems.AddAsync(newItem);
        await _context.SaveChangesAsync();
        return newItem.Id;
    }

    public async Task<Guid> Complete(Guid id)
    {
        var item = _context.TodoItems.Find(id);
        item.Completed = DateTime.UtcNow;
        _context.Update(item);
        await _context.SaveChangesAsync();
        return item.Id;
    }
}