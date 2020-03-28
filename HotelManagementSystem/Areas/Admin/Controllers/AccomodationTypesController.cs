using HotelManagementSystem.Areas.Admin.ViewModel;
using HotelManagementSystem.Models;
using System.Linq;
using System.Web.Mvc;

namespace HotelManagementSystem.Areas.Admin.Controllers
{
    public class AccomodationTypesController : Controller
    {  
         ApplicationDbContext _context = new ApplicationDbContext();
        // GET: Admin/AccomodationTypes
        public ActionResult Index()
        {
            var accomodationTypes = _context.AccomodationTypes.ToList();
            var viewModel = new  AccomodationTypeViewModel()
            {
                AccomodationTypes = accomodationTypes
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Action()
        {
            var model = new AccomodationTypeActionViewModel();
            return PartialView("_Action",model);
        }

        [HttpPost]
        public ActionResult Action(AccomodationType model)
        {
            var accomodationType = new AccomodationType();

            accomodationType.Name = model.Name;
            accomodationType.Description = model.Description;

            _context.AccomodationTypes.Add(accomodationType);
            _context.SaveChanges();

            return Json(new {success = true}, JsonRequestBehavior.AllowGet);
        }
    }
}