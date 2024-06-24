using Assignment.Application.ApiServices;
using Moq;

namespace Assignment.Application.WeatherForecast.Queries.GetWeather;
[Common.Security.Authorize]
public class GetWeatherTemperatureForCityQuery : IRequest<int>
{
    public string? CityName { get; set; }
}

public class GetWeatherTemperatureForCityQueryHandler : IRequestHandler<GetWeatherTemperatureForCityQuery, int>
{
    private readonly IWeatherApiService _weatherApiService;

    public GetWeatherTemperatureForCityQueryHandler()
    {
        var weatherApiServiceMock = new Mock<IWeatherApiService>();

        weatherApiServiceMock.Setup(t => t.GetTemperatureForCity(It.IsAny<string>())).Returns(Task.FromResult(new Random().Next(-10, 30)));

        _weatherApiService = weatherApiServiceMock.Object;
    }

    public async Task<int> Handle(GetWeatherTemperatureForCityQuery request, CancellationToken cancellationToken)
    {
        return await _weatherApiService.GetTemperatureForCity(request.CityName!);
    }
}
