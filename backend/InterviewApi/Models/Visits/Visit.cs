using InterviewApi.Models.Customers;
using InterviewApi.Models.Hotels;

namespace InterviewApi.Models.Visits
{
    public class Visit
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int HotelId { get; set; }
        public DateTime VisitDate { get; set; }
    }

    public class VisitView
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int HotelId { get; set; }
        public DateTime VisitDate { get; set; }

        public Customer Customer { get; set; }
        public Hotel Hotel { get; set; }
    }

    public class VisitData
    {
        public List<Visit> Visitations { get; set; } = new List<Visit>();
    }

}
