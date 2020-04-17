using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagementSystem.Models
{
    public class Picture
    {
        public int  Id { get; set; }
        public string Url { get; set; }

        public int? AccomodationId { get; set; }
        public Accomodation Accomodation { get; set; }
        public int? AccomodationPackageId { get; set; }
        public AccomodationPackage AccomodationPackage { get; set; }
    }
}