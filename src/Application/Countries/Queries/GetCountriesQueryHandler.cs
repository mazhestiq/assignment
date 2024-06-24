using Assignment.Application.Common.Interfaces;
using Assignment.Application.Common.Security;

namespace Assignment.Application.Countries.Queries;

[Authorize]
public class GetCountriesQuery : IRequest<IList<CountryDto>>;

public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, IList<CountryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCountriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IList<CountryDto>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Countries
            .AsNoTracking()
            .ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
