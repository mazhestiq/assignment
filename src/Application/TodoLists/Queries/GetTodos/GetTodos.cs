using Assignment.Application.CacheService;
using Assignment.Application.Common.Interfaces;
using Assignment.Application.Common.Security;

namespace Assignment.Application.TodoLists.Queries.GetTodos;

[Authorize]
public record GetTodosQuery : IRequest<IList<TodoListDto>>;

public class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, IList<TodoListDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public GetTodosQueryHandler(IApplicationDbContext context, IMapper mapper, ICacheService cacheService)
    {
        _context = context;
        _mapper = mapper;
        _cacheService = cacheService;
    }

    public async Task<IList<TodoListDto>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
    {
        var result = _cacheService.Get<IList<TodoListDto>>(nameof(GetTodosQueryHandler));

        if (result != null)
        {
            return result;
        }

        result = await _context.TodoLists
            .AsNoTracking()
            .ProjectTo<TodoListDto>(_mapper.ConfigurationProvider)
            .OrderBy(t => t.Title)
            .ToListAsync(cancellationToken);

        _cacheService.Set(nameof(GetTodosQueryHandler), result);

        return result;
    }
}
