namespace Assignment.Application.Countries.Queries.GetCountries;

public class CountryDto
{
    public string? Name { get; init; }

    public IList<CityDto> Cities { get; init; } = Array.Empty<CityDto>();
}