using System.Linq;
using System.Web.Mvc;
using HotelManagementSystem.Areas.Admin.ViewModel;
using HotelManagementSystem.Models;

namespace HotelManagementSystem.Areas.Admin.Controllers
{
    public class AccomodationTypesController : Controller
    {  
         ApplicationDbContext _context = new ApplicationDbContext();
        // GET: Admin/AccomodationTypes
        public ActionResult Index(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                var accomodationTypes = _context.AccomodationTypes.ToList();
                var viewModel = new AccomodationTypeViewModel()
                {
                    AccomodationTypes = accomodationTypes
                };

                return View(viewModel);
            }
            else
            {
                var accomodationTypes = _context.AccomodationTypes.Where(a => a.Name.Contains(searchTerm)).ToList();

                var viewModel = new AccomodationTypeViewModel()
                {
                    AccomodationTypes = accomodationTypes,
                    SearchTerm = searchTerm
                };
                return View(viewModel);
            }
            
        }

        [HttpGet]
        public ActionResult Action(int? id)
        {
            if (id == null)                                        //create form
            {
                var model = new AccomodationTypeActionViewModel();
                return PartialView("_Action", model);
            }
            else                                                   // edit form
            {
                var accomodationType = _context.AccomodationTypes.Find(id);
                var model = new AccomodationTypeActionViewModel()
                {
                    Id = accomodationType.Id,
                    Name = accomodationType.Name,
                    Description = accomodationType.Description
                };
                return PartialView("_Action", model);
            }
            
        }

        [HttpPost]
        public ActionResult Action(AccomodationType model)
        {
            if (model.Id > 0)                           // edit a accommodation type
            {
                var accomodationType = _context.AccomodationTypes.Find(model.Id);
                accomodationType.Name = model.Name;
                accomodationType.Description = model.Description;
            }
            else                                          // create a new accommodation type
            {
                var accomodationType = new AccomodationType();

                accomodationType.Name = model.Name;
                accomodationType.Description = model.Description;

                _context.AccomodationTypes.Add(accomodationType);
            }

            _context.SaveChanges();

            return Json(new {success = true}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var accomodationType = _context.AccomodationTypes.Find(id);
            var model = new AccomodationTypeActionViewModel()
            {
                Id = accomodationType.Id
                
            };
            return PartialView("_Delete", model);
        }

        [HttpPost]
        public ActionResult Delete(AccomodationType model)
        {

            var accomodationType = _context.AccomodationTypes.Find(model.Id);

            _context.AccomodationTypes.Remove(accomodationType);
            _context.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}