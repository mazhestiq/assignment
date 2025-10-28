using Microsoft.AspNetCore.Mvc;
using InterviewApi.Models.Customers;
using InterviewApi.Helper;
using InterviewApi.Models.Visits;

namespace InterviewApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelsController : ControllerBase
{
    [HttpGet]
    public ActionResult<VisitView[]> GetHotels([FromRoute] int customerId)
    {
        var hotels = Singleton.ReadHotelsFromJson();

        return Ok(hotels);
    }
}