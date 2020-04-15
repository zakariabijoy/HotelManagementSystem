using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HotelManagementSystem.Areas.Admin.ViewModel;
using HotelManagementSystem.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;

namespace HotelManagementSystem.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();

            }
            private set
            {
                _signInManager = value;

            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); 

            }
            private set
            {
                _userManager = value;

            }
        }
        public ApplicationRoleManager RoleManager
        {
            get { return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>(); }
            private set { _roleManager = value; }
        }
        public UsersController()
        {

        }

        public UsersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {

            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }
        
        // GET: Admin/AccomodationTypes
        public async Task<ActionResult> Index(string searchTerm, string roleId, int? page)
        {
            page = page ?? 1;
            var roles = RoleManager.Roles.ToList();
            var model = new UsersViewModel()
            {
                Users =  await Users(searchTerm, roleId, page.Value),
                Roles = roles,
                SearchTerm = searchTerm,
                RoleId = roleId
            };

            return View(model);
        }
        [NonAction]
        public async Task<IPagedList<ApplicationUser>> Users(string searchTerm, string roleId, int page)
        {
            var users = UserManager.Users.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(a => a.Email.Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(roleId))
            {
                var role = await RoleManager.FindByIdAsync(roleId);
                var userIds = role.Users.Select(u => u.UserId).ToList();
                users = users.Where(u=>userIds.Contains(u.Id));
            }


            return users.ToList().ToPagedList(page, 3);
        }

        [HttpGet]
        public async Task<ActionResult> Action(string id)
        {
            var roles = RoleManager.Roles.ToList();
            if (string.IsNullOrEmpty(id))                                        //create form
            {
                var model = new UsersActionViewModel()
                {
                    Roles = roles
                };
                return PartialView("_Action", model);
            }
            else                                                   // edit form
            {
                var user = await UserManager.FindByIdAsync(id);
                var model = new UsersActionViewModel()
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    UserName = user.UserName,
                    Address = user.Address,
                    Country = user.Country,
                    City = user.City,
                    
                    Roles = roles
                };
                return PartialView("_Action", model);
            }

        }

        [HttpPost]
        public async Task<ActionResult> Action(UsersActionViewModel model)
        {
            IdentityResult result = null;
            if (!string.IsNullOrEmpty(model.Id))                           // edit a accommodation type
            {
               
                var user = await UserManager.FindByIdAsync(model.Id);
                user.FullName = model.FullName;
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.City = model.City;
                user.Address = model.Address;
                user.Country = model.Country;

                result = await UserManager.UpdateAsync(user);
            }
            else                                          // create a new accommodation type
            {
                var user = new ApplicationUser();

                user.FullName = model.FullName;
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.City = model.City;
                user.Address = model.Address;
                user.Country = model.Country;

                result = await UserManager.CreateAsync(user);
            }


            return Json(new { success = result.Succeeded, message = string.Join("</br>",result.Errors) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            var model = new UsersActionViewModel()
            {
                Id = user.Id

            };
            return PartialView("_Delete", model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(UsersActionViewModel model)
        {
            IdentityResult result = null;
            if (!string.IsNullOrEmpty(model.Id))
            {
                var user = await UserManager.FindByIdAsync(model.Id);
                result = await UserManager.DeleteAsync(user);

                return Json(new {success = result.Succeeded, message = string.Join("</br>", result.Errors)},
                    JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "invalid user" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public async Task<ActionResult> UserRoles(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            var userRoleIds = user.Roles.Select(r => r.RoleId).ToList();
            var userRoles = RoleManager.Roles.Where(r => userRoleIds.Contains(r.Id)).ToList();
            var roles = RoleManager.Roles.Where(r=>!userRoleIds.Contains(r.Id)).ToList();

            var model = new UserRolesViewModel()
            {
                UserId = user.Id,
                Roles = roles,
                UserRoles = userRoles
            };

            return PartialView("_UserRoles", model);
          

        }

        [HttpPost]
        public async Task<ActionResult> UserRoleOperation(string userId, string roleId, bool isDelete = false)
        {
            IdentityResult result = null;
            var user = await UserManager.FindByIdAsync(userId);
            var role =await RoleManager.FindByIdAsync(roleId);

            if (user != null && role != null)
            {
                if (!isDelete)
                {
                    result = await UserManager.AddToRoleAsync(userId, role.Name);
                }
                else
                {
                    result = await UserManager.RemoveFromRoleAsync(userId, role.Name);
                }

                return Json(new { success = result.Succeeded, message = string.Join("</br>", result.Errors) });
            }
            else
            {
                return Json(new { success = false, message = "Invalid operation" });
            }
            
            
        }
        
        
    }
}