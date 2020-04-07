using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using HotelManagementSystem.Areas.Admin.ViewModel;
using HotelManagementSystem.Models;

namespace HotelManagementSystem.Areas.Admin.Controllers
{
    public class AccomodationPackagesController : Controller
    {
        // GET: Admin/AccomodationPackages
        ApplicationDbContext _context = new ApplicationDbContext();
        // GET: Admin/AccomodationTypes
        public ActionResult Index(string searchTerm, int? accomodationTypeId, int? page)
        {
            int recordSize = 3;
            page = page ?? 1;
            var accomodationTypes = _context.AccomodationTypes.ToList();
            var model = new AccomodationPackagesViewModel()
            {
                AccomodationPackages = AccomodationPackages(searchTerm,accomodationTypeId, page.Value),
                AccomodationTypes = accomodationTypes,
                SearchTerm = searchTerm,
                AccomodationTypeId = accomodationTypeId
            };

            return View(model);
        }

        public IPagedList<AccomodationPackage> AccomodationPackages(string searchTerm, int? accomodationTypeId,int page )
        {
            var accomodationPackages= _context.AccomodationPackages.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodationPackages = accomodationPackages.Where(a => a.Name.Contains(searchTerm));
            }

            if (accomodationTypeId !=null && accomodationTypeId>0)
            {
                accomodationPackages = accomodationPackages.Where(a => a.AccomodationTypeId == accomodationTypeId);
            }

            
            return accomodationPackages.ToList().ToPagedList(page,3);
        }

        [HttpGet]
        public ActionResult Action(int? id)
        {
          var accomodationTypes = _context.AccomodationTypes.ToList();
            if (id == null)                                        //create form
            {
                var model = new AccomodationPackagesActionViewModel()
                {
                    AccomodationTypes = accomodationTypes
                };
                return PartialView("_Action", model);
            }
            else                                                   // edit form
            {
                var accomodationPackage = _context.AccomodationPackages.Find(id);
                var model = new AccomodationPackagesActionViewModel()
                {
                    Id = accomodationPackage.Id,
                    Name = accomodationPackage.Name,
                    AccomodationTypeId = accomodationPackage.AccomodationTypeId,
                    NoOfRoom = accomodationPackage.NoOfRoom,
                    FeePerNight = accomodationPackage.FeePerNight,
                    AccomodationTypes = accomodationTypes
                };
                return PartialView("_Action", model);
            }

        }

        [HttpPost]
        public ActionResult Action(AccomodationPackage model)
        {
            if (model.Id > 0)                           // edit a accommodation type
            {
                var accomodationPackage = _context.AccomodationPackages.Find(model.Id);

                accomodationPackage.Name = model.Name;
                accomodationPackage.AccomodationTypeId = model.AccomodationTypeId;
                accomodationPackage.FeePerNight = model.FeePerNight;
                accomodationPackage.NoOfRoom = model.NoOfRoom;
            }
            else                                          // create a new accommodation type
            {
                var accomodationPackage = new AccomodationPackage();

                accomodationPackage.Name = model.Name;
                accomodationPackage.AccomodationTypeId = model.AccomodationTypeId;
                accomodationPackage.FeePerNight = model.FeePerNight;
                accomodationPackage.NoOfRoom = model.NoOfRoom;

                _context.AccomodationPackages.Add(accomodationPackage);
            }

            _context.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var accomodationPackage = _context.AccomodationPackages.Find(id);
            var model = new AccomodationPackagesActionViewModel()
            {
                Id = accomodationPackage.Id

            };
            return PartialView("_Delete", model);
        }

        [HttpPost]
        public ActionResult Delete(AccomodationType model)
        {

            var accomodationPackage = _context.AccomodationPackages.Find(model.Id);

            _context.AccomodationPackages.Remove(accomodationPackage);
            _context.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}