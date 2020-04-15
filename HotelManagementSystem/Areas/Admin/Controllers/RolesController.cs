using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HotelManagementSystem.Areas.Admin.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using PagedList;

namespace HotelManagementSystem.Areas.Admin.Controllers
{
    public class RolesController : Controller
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public ApplicationRoleManager _roleManager;

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        public ApplicationRoleManager RoleManager
        {
            get { return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>(); }
            private set { _roleManager = value; }
        }

        public RolesController()
        {

        }

        public RolesController(ApplicationUserManager userManager, ApplicationSignInManager signInManager,
            ApplicationRoleManager roleManager)
        {

            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        // GET: Admin/Roles
        public ActionResult Index(string searchTerm, int? page)
        {
            page = page ?? 1;

            var model = new RolesViewModel()
            {
                Roles = Roles(searchTerm, page.Value),
                SearchTerm = searchTerm
            };

            return View(model);
        }

        [NonAction]
        public IPagedList<IdentityRole> Roles(string searchTerm, int page)
        {
            var roles = RoleManager.Roles.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                roles = roles.Where(a => a.Name.Contains(searchTerm));
            }


            return roles.ToList().ToPagedList(page, 3);
        }

        [HttpGet]
        public async Task<ActionResult> Action(string id)
        {
           
            if (string.IsNullOrEmpty(id)) //create form
            {
                var model = new RoleActionViewModel();
                return PartialView("_Action", model);
            }
            else // edit form
            {
                var role = await RoleManager.FindByIdAsync(id);
                var model = new RoleActionViewModel()
                {
                    Id = role.Id,
                    Name = role.Name
                };
                return PartialView("_Action", model);
            }

        }

        [HttpPost]
        public async Task<ActionResult> Action(RoleActionViewModel model)
        {
            IdentityResult result = null;
            if (!string.IsNullOrEmpty(model.Id)) // edit a accommodation type
            {

                var role = await RoleManager.FindByIdAsync(model.Id);
                role.Name = model.Name;
                

                result = await RoleManager.UpdateAsync(role);
            }
            else // create a new accommodation type
            {
                var role = new IdentityRole();

                role.Name = model.Name;

                result = await RoleManager.CreateAsync(role);
            }


            return Json(new {success = result.Succeeded, message = string.Join("</br>", result.Errors)},
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            var model = new RoleActionViewModel()
            {
                Id = role.Id

            };
            return PartialView("_Delete", model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(RoleActionViewModel model)
        {
            IdentityResult result = null;
            if (!string.IsNullOrEmpty(model.Id))
            {
                var role = await RoleManager.FindByIdAsync(model.Id);
                result = await RoleManager.DeleteAsync(role);

                return Json(new {success = result.Succeeded, message = string.Join("</br>", result.Errors)},
                    JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new {success = false, message = "invalid user"}, JsonRequestBehavior.AllowGet);
            }



        }
    }
}