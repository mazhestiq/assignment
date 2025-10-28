using InterviewApi.Models.Customers;
using InterviewApi.Models.Hotels;
using InterviewApi.Models.Visits;
using System.Text.Json;

namespace InterviewApi.Helper
{
    public class Singleton
    {
        private static readonly string _customersDataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "customers.json");
        private static readonly string _visitDataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "visitations.json");
        private static readonly string _hotelsDataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "hotels.json");
        /// <summary>
        /// Helper method to read visits from JSON file
        /// </summary>
        public static List<Visit> ReadVisitsFromJson()
        {
            try
            {
                if (!System.IO.File.Exists(_visitDataPath))
                    return [];

                var json = System.IO.File.ReadAllText(_visitDataPath);
                var data = JsonSerializer.Deserialize<VisitData>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return data?.Visitations ?? [];
            }
            catch
            {
                return [];
            }
        }

        /// <summary>
        /// Helper method to write visits to JSON file
        /// </summary>
        public static void WriteVisitsToJson(List<Visit> visits)
        {
            try
            {
                var data = new VisitData { Visitations = visits };
                var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText(_visitDataPath, json);
            }
            catch
            {
                // Handle error as needed
            }
        }

        /// <summary>
        /// Helper method to read customers from JSON file
        /// </summary>
        public static List<Customer> ReadCustomersFromJson()
        {
            try
            {
                if (!System.IO.File.Exists(_customersDataPath))
                    return [];

                var json = System.IO.File.ReadAllText(_customersDataPath);
                var data = JsonSerializer.Deserialize<CustomerData>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return data?.Customers ?? [];
            }
            catch
            {
                return [];
            }
        }

        /// <summary>
        /// Helper method to write customers to JSON file
        /// </summary>
        public static void WriteCustomersToJson(List<Customer> customers)
        {
            try
            {
                var data = new CustomerData { Customers = customers };
                var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText(_customersDataPath, json);
            }
            catch
            {
                // Handle error as needed
            }
        }


        /// <summary>
        /// Helper method to read customers from JSON file
        /// </summary>
        public static List<Hotel> ReadHotelsFromJson()
        {
            try
            {
                if (!System.IO.File.Exists(_hotelsDataPath))
                    return [];

                var json = System.IO.File.ReadAllText(_hotelsDataPath);
                var data = JsonSerializer.Deserialize<HotelData>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return data?.Hotels ?? [];
            }
            catch
            {
                return [];
            }
        }

        /// <summary>
        /// Helper method to write customers to JSON file
        /// </summary>
        public static void WriteHotelsToJson(List<Hotel> hotels)
        {
            try
            {
                var data = new HotelData
                {

                    Hotels= hotels
                };
                var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText(_hotelsDataPath, json);
            }
            catch
            {
                // Handle error as needed
            }
        }
    }
}
