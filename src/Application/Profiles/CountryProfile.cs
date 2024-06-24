using Assignment.Application.Countries.Queries.GetCountries;
using Assignment.Domain.Entities;

namespace Assignment.Application.Profiles;

public class CountryProfile : Profile
{
    public CountryProfile()
    {
        CreateMap<Country, CountryDto>().ReverseMap();
        
    }
}