using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace HotelManagementSystem.Models
{
    public class Accomodation
    {
        public int Id { get; set; }

        public int AccomodationPackageId { get; set; }  
        public AccomodationPackage AccomodationPackage { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public List<Picture> Pictures { get; set; }
    }
}