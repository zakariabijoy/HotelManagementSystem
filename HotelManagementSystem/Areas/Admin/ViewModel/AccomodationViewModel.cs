using System.Collections.Generic;
using System.Web;
using HotelManagementSystem.Models;
using PagedList;

namespace HotelManagementSystem.Areas.Admin.ViewModel
{
    public class AccomodationViewModel
    {
        public IPagedList<Accomodation> Accomodations { get; set; }
        public IEnumerable<AccomodationPackage> AccomodationPackages { get; set; }
        public int? AccomodationPackageId { get; set; }
        public string SearchTerm { get; set; }


    }
    public class AccomodationActionViewModel
    {
        public int Id { get; set; }

        public int AccomodationPackageId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public HttpPostedFileBase[] PictureFiles { get; set; }
        public List<Picture> Pictures { get; set; }

        public IEnumerable<AccomodationPackage> AccomodationPackages { get; set; }

    }
}