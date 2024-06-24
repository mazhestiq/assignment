using Assignment.Application.TodoLists.Queries.GetTodos;
using Assignment.Domain.Entities;

namespace Assignment.Application.Profiles;
public class TodoListProfile : Profile
{
    public TodoListProfile()
    {
        CreateMap< TodoList,TodoListDto> ().ReverseMap();
        CreateMap<TodoItem, TodoItemDto>().ReverseMap();
    }
}
