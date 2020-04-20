using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelManagementSystem.Areas.Admin.ViewModel;
using HotelManagementSystem.Models;
using HotelManagementSystem.ViewModels;

namespace HotelManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult Index(int? accomodationTypeId)
        {
            if (accomodationTypeId == null)
            {
                var model = new HomeViewmodel()
                {
                    AccomodationTypes = _context.AccomodationTypes.ToList(),
                    AccomodationPackages = _context.AccomodationPackages.ToList()
                };
                return View(model);
            }
            else
            {
                var model = new HomeViewmodel()
                {
                    AccomodationTypes = _context.AccomodationTypes.ToList(),
                    AccomodationPackages = _context.AccomodationPackages.Where(a=>a.AccomodationTypeId==accomodationTypeId).ToList()
                };
                return View(model);
            }
            
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AccomodationPackageDetails(int accomodationpackageid)
        {
            var accomodationPackage = _context.AccomodationPackages.Include(a=>a.AccomodationType).FirstOrDefault(a => a.Id == accomodationpackageid);
            accomodationPackage.Pictures =
                _context.Pictures.Where(p => p.AccomodationPackageId == accomodationpackageid).ToList();
            return View(accomodationPackage);
        }

        
    }
}