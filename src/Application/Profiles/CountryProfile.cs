using Assignment.Application.Countries.Queries;
using Assignment.Domain.Entities;

namespace Assignment.Application.Profiles;

public class CountryProfile : Profile
{
    public CountryProfile()
    {
        CreateMap<Country, CountryDto>().ReverseMap();
        
    }
}