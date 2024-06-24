using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;

namespace Assignment.Application.ApiServices;
public interface IWeatherApiService
{
    [Get("/{city}/temperature")]
    Task<int> GetTemperatureForCity(string city);
}
