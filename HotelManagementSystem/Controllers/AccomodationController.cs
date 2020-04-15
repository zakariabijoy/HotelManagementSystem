using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelManagementSystem.Models;
using HotelManagementSystem.ViewModels;

namespace HotelManagementSystem.Controllers
{
    public class AccomodationController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        // GET: Accomodations
        public ActionResult Index(int accomodationTypeId, int? accomodationPackageId)
        {
            var model = new AccomodationsViewModel();

            model.AccomodationType = _context.AccomodationTypes.Find(accomodationTypeId);
            
            model.AccomodationPackages = _context.AccomodationPackages
                .Where(aP => aP.AccomodationTypeId == accomodationTypeId).ToList();

            model.SelectedAccomodationPackageId = accomodationPackageId.HasValue
                ? accomodationPackageId
                : model.AccomodationPackages.First().Id;

            model.Accomodations = _context.Accomodations.Where(a => a.AccomodationPackageId == model.SelectedAccomodationPackageId).ToList();

            return View(model);
        }
    }
}