using System.Collections.Generic;
using System.Web;
using HotelManagementSystem.Models;
using PagedList;

namespace HotelManagementSystem.Areas.Admin.ViewModel
{
    public class AccomodationPackagesViewModel
    {
        public IPagedList<AccomodationPackage> AccomodationPackages { get; set; }
        public IEnumerable<AccomodationType> AccomodationTypes { get; set; }
        public int? AccomodationTypeId { get; set; }
        public string SearchTerm { get; set; }
    }

    public class AccomodationPackagesActionViewModel
    {
        public int Id { get; set; }
        public int AccomodationTypeId { get; set; }
        public string Name { get; set; }
        public int NoOfRoom { get; set; }
        public decimal FeePerNight { get; set; }
        public HttpPostedFileBase[] PictureFiles { get; set; }
        public List<Picture> Pictures { get; set; }

        public IEnumerable<AccomodationType> AccomodationTypes { get; set; }

    }
}