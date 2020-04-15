using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HotelManagementSystem.Models;

namespace HotelManagementSystem.ViewModels
{
    public class AccomodationsViewModel
    {
        public AccomodationType AccomodationType { get; set; }
        public IEnumerable<AccomodationPackage> AccomodationPackages { get; set; }
        public IEnumerable<Accomodation> Accomodations { get; set; }
        public int? SelectedAccomodationPackageId { get; set; }

    }
}