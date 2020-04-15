using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;

namespace HotelManagementSystem.Areas.Admin.ViewModel
{
    public class RolesViewModel
    {
        public IPagedList<IdentityRole> Roles { get; set; }

        public string SearchTerm { get; set; }


    }
    public class RoleActionViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

    }
}