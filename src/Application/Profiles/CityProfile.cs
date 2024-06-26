﻿using Assignment.Application.Countries.Queries.GetCountries;
using Assignment.Domain.Entities;

namespace Assignment.Application.Profiles;

public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<City, CityDto>().ReverseMap();

    }
}