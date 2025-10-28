using InterviewApi.Models.Visits;

namespace InterviewApi.Models.Hotels
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public double Rating { get; set; }
        public string Description { get; set; }
    }


    public class HotelData
    {
        public List<Hotel> Hotels { get; set; } = new List<Hotel>();
    }
}
