using Assignment.Application.TodoLists.Queries.GetTodos;
using Assignment.Domain.Entities;

namespace Assignment.Application.Profiles;
public class TodoListProfile : Profile
{
    public TodoListProfile()
    {
        CreateMap< Assignment.Domain.Entities.TodoList,Assignment.Application.TodoLists.Queries.GetTodos.TodoListDto> ().ReverseMap();
        CreateMap<Assignment.Domain.Entities.TodoItem, Assignment.Application.TodoLists.Queries.GetTodos.TodoItemDto>().ReverseMap();
    }
}
