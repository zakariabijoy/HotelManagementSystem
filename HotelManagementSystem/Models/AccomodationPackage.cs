using System.Collections.Generic;

namespace HotelManagementSystem.Models
{
    public class AccomodationPackage
    {
        public int Id { get; set; }

        public int AccomodationTypeId { get; set; } 
        public AccomodationType AccomodationType { get; set; }

        public string Name { get; set; }
        public int NoOfRoom { get; set; }
        public decimal FeePerNight { get; set; }

        public List<Picture> Pictures { get; set; }   
    }
}