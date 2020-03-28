using System;

namespace HotelManagementSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int AccomodationId { get; set; }
        public Accomodation Accomodation { get; set; }

        public DateTime FromDate { get; set; }
        /// <summary>
        /// No Of Stay Nights
        /// </summary>
        public int Duration { get; set; }

       
    }
}