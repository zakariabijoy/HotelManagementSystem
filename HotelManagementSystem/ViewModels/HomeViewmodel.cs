using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HotelManagementSystem.Models;

namespace HotelManagementSystem.ViewModels
{
    public class HomeViewmodel
    {
        public IEnumerable<AccomodationType> AccomodationTypes { get; set; }
        public IEnumerable<AccomodationPackage> AccomodationPackages { get; set; }
    }
}