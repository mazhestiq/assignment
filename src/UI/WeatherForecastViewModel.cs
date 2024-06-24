using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Assignment.Application.Countries.Queries;
using Assignment.Application.TodoItems.Commands.DoneTodoItem;
using Assignment.Application.TodoLists.Queries.GetTodos;
using Assignment.Domain.Entities;
using Caliburn.Micro;
using MediatR;

namespace Assignment.UI;
public class WeatherForecastViewModel : Screen
{
    private readonly ISender _sender;

    private IList<CountryDto> _countries;
    public IList<CountryDto> Countries
    {
        get
        {
            return _countries;
        }
        set
        {
            _countries = value;
            NotifyOfPropertyChange(() => Countries);
        }
    }
    
    private CountryDto _selectedCountry;
    public CountryDto SelectedCountry
    {
        get => _selectedCountry;
        set
        {
            _selectedCountry = value;
            RefreshCitiesList(_selectedCountry);
            NotifyOfPropertyChange(() => SelectedCountry);
        }
    }

    
    private IList<CityDto> cities;
    public IList<CityDto> Cities
    {
        get
        {
            return cities;
        }
        set
        {
            cities = value;
            NotifyOfPropertyChange(() => Cities);
        }
    }

    private CityDto _selectedCity;
    public CityDto SelectedCity
    {
        get => _selectedCity;
        set
        {
            _selectedCity = value;
            RefreshTemperature(_selectedCity);
            NotifyOfPropertyChange(() => SelectedCity);
        }
    }

    private string _temperature;
    public string Temperature
    {
        get => _temperature;
        set
        {
            _temperature = value;
            NotifyOfPropertyChange(() => Temperature);
        }
    }

    public WeatherForecastViewModel(ISender sender)
    {
        _sender = sender;
        Initialize();
    }

    private async void Initialize()
    {
        await RefreshCountryList();
    }

    private async Task RefreshCountryList()
    {
        Countries = await _sender.Send(new GetCountriesQuery());
    }

    private void RefreshCitiesList(CountryDto country)
    {
        Cities = country.Cities;
    }

    private void RefreshTemperature(CityDto city)
    {
        if (city == null)
        {
            Temperature = $"Please select city.";
            return;
        }

        Temperature = $"{city.Name} temperature is 10";
    }
}
