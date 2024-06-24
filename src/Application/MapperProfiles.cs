using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment.Application.Profiles;

namespace Assignment.Application;
public static class MapperProfiles
{
    public static void UseApplicationProfiles(this IMapperConfigurationExpression config)
    {
        config.AddProfile<TodoListProfile>();
        config.AddProfile<CityProfile>();
        config.AddProfile<CountryProfile>();
    }
}
