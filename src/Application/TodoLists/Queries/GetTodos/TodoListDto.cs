using Assignment.Domain.Entities;

namespace Assignment.Application.TodoLists.Queries.GetTodos;

public class TodoListDto
{
    public int Id { get; init; }

    public string? Title { get; init; }

    public string? Colour { get; init; }

    public IList<TodoItemDto> Items { get; init; } = Array.Empty<TodoItemDto>();
}
