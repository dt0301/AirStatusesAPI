using System.ComponentModel.DataAnnotations;

namespace AirStatusesDomain
{
    public class Flight
    {
        [Key]
        public int Id { get; set; }
         
        [MaxLength(256)]
        public string Origin { get; set; }

        [MaxLength(256)]
        public string Destination { get; set; } 

        public DateTimeOffset Departure { get; set; }
        public DateTimeOffset Arrival { get; set; }
        public FlightStatus Status { get; set; }
    }
}
