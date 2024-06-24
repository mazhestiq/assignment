namespace Assignment.Application.Common.Interfaces;
public interface IWeatherForecastApi
{
    int GetTemperature(string cityName);
}
