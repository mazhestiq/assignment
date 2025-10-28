using InterviewApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using InterviewApi.Helper;
using InterviewApi.Models.Customers;
using InterviewApi.Models.Hotels;
using InterviewApi.Models.Visits;

namespace InterviewApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VisitsController : ControllerBase
{
    [HttpGet]
    public ActionResult<VisitView[]> GetVisits()
    {
        var visits = Singleton.ReadVisitsFromJson();

        var customers = Singleton.ReadCustomersFromJson().ToDictionary(x=>x.Id, x=>x);
        var hotels = Singleton.ReadHotelsFromJson().ToDictionary(x => x.Id, x => x);

        var result = visits.ToArray().Select(x => new VisitView
        {
            CustomerId = x.CustomerId,
            Id = x.Id,
            HotelId = x.HotelId,
            VisitDate = x.VisitDate,
            Customer = customers.ContainsKey(x.CustomerId) ? new Customer
            {
                Name = customers[x.CustomerId].Name,
                Email = customers[x.CustomerId].Email,
                RegistrationDate = customers[x.CustomerId].RegistrationDate,
                TotalPurchases = customers[x.CustomerId].TotalPurchases
            } : new Customer(),
            Hotel = hotels.ContainsKey(x.HotelId) ? new Hotel
            {
                Name = hotels[x.CustomerId].Name,
                Description = hotels[x.CustomerId].Description,
                Id = hotels[x.CustomerId].Id,
                Location = hotels[x.CustomerId].Location,
                Rating = hotels[x.CustomerId].Rating
            } : new Hotel(),
        });

        return Ok(result);
    }

    [HttpGet("customer/{customerId}")]
    public ActionResult<VisitView[]> GetCutomerVisits([FromRoute] int customerId)
    {
        var visits = Singleton.ReadVisitsFromJson();
        var customers = Singleton.ReadCustomersFromJson().ToDictionary(x => x.Id, x => x);
        var hotels = Singleton.ReadHotelsFromJson().ToDictionary(x => x.Id, x => x);

        var result = visits.Where(x => x.CustomerId == customerId).ToArray().Select(x=>new VisitView
        {
            CustomerId = x.CustomerId,
            Id = x.Id,
            HotelId = x.HotelId,
            VisitDate = x.VisitDate,
            Customer = customers.ContainsKey(x.CustomerId) ? new Customer
            {
                Name = customers[x.CustomerId].Name,
                Email = customers[x.CustomerId].Email,
                RegistrationDate = customers[x.CustomerId].RegistrationDate,
                TotalPurchases = customers[x.CustomerId].TotalPurchases
            } : new Customer(),
            Hotel = hotels.ContainsKey(x.HotelId) ? new Hotel
            {
                Name = hotels[x.CustomerId].Name,
                Description = hotels[x.CustomerId].Description,
                Id = hotels[x.CustomerId].Id,
                Location = hotels[x.CustomerId].Location,
                Rating = hotels[x.CustomerId].Rating
            } : new Hotel(),
        });

        return Ok(result);
    }
}