using System.Collections.Generic;
using HotelManagementSystem.Models;

namespace HotelManagementSystem.Areas.Admin.ViewModel
{
    public class AccomodationTypeViewModel
    {
        public IEnumerable<AccomodationType> AccomodationTypes { get; set; }
        public string SearchTerm { get; set; }
    }

    public class AccomodationTypeActionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
       
    }

}