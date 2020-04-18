using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using HotelManagementSystem.Areas.Admin.ViewModel;
using HotelManagementSystem.Models;
using PagedList;

namespace HotelManagementSystem.Areas.Admin.Controllers
{
    public class AccomodationsController : Controller
    {
        // GET: Admin/Accomodation
        ApplicationDbContext _context = new ApplicationDbContext();
        // GET: Admin/AccomodationTypes
        public ActionResult Index(string searchTerm, int? accomodationPackageId, int? page)
        {
            page = page ?? 1;
            var accomodationPackages = _context.AccomodationPackages.ToList();
            var model = new AccomodationViewModel()
            {
                Accomodations = Accomodations(searchTerm, accomodationPackageId, page.Value),
                AccomodationPackages = accomodationPackages,
                SearchTerm = searchTerm,
                AccomodationPackageId = accomodationPackageId
            };

            return View(model);
        }
        [NonAction]
        public IPagedList<Accomodation> Accomodations(string searchTerm, int? accomodationPackageId, int page)
        {
            var accomodations = _context.Accomodations.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodations = accomodations.Where(a => a.Name.Contains(searchTerm));
            }

            if (accomodationPackageId != null && accomodationPackageId > 0)
            {
                accomodations = accomodations.Where(a => a.AccomodationPackageId == accomodationPackageId);
            }


            return accomodations.ToList().ToPagedList(page, 3);
        }

        [HttpGet]
        public ActionResult Action(int? id)
        {
            var accomodationPackages = _context.AccomodationPackages.ToList();
            if (id == null)                                        //create form
            {
                var model = new AccomodationActionViewModel()
                {
                    AccomodationPackages = accomodationPackages
                };
                return PartialView("_Action", model);
            }
            else                                                   // edit form
            {
                var accomodation = _context.Accomodations.Find(id);
                var pictures = _context.Pictures.Where(p => p.AccomodationId == accomodation.Id).ToList();
                var model = new AccomodationActionViewModel()
                {
                    Id = accomodation.Id,
                    Name = accomodation.Name,
                    Description = accomodation.Description,
                    AccomodationPackageId = accomodation.AccomodationPackageId,
                    AccomodationPackages = accomodationPackages,
                    Pictures = pictures
                };
                return PartialView("_Action", model);
            }

        }

        [HttpPost]
        public ActionResult Action(AccomodationActionViewModel model)
        {
            if (model.Id > 0)                           // edit a accommodation type
            {
                var accomodation = _context.Accomodations.Find(model.Id);

                accomodation.Name = model.Name;
                accomodation.AccomodationPackageId = model.AccomodationPackageId;
                accomodation.Description = model.Description;

                if (model.PictureFiles[0] != null)
                {
                    foreach (var pictureFile in model.PictureFiles)
                    {
                        string directoryPath = "~/images/UploadedImages/";

                        string fileName = Path.GetFileNameWithoutExtension(pictureFile.FileName);

                        string extension = Path.GetExtension(pictureFile.FileName);
                        fileName = fileName + "-" + DateTime.Now.ToString("yymmssfff") + Guid.NewGuid() + extension;
                        var ServerSavePath = Path.Combine(Server.MapPath(directoryPath) + fileName);

                        //Save file to server folder  
                        pictureFile.SaveAs(ServerSavePath);
                        //assigning file uploaded status to ViewBag for showing message to user.  

                        var picture = new Picture();
                        picture.Url = (string)directoryPath + fileName;
                        picture.AccomodationId = model.Id;
                        _context.Pictures.Add(picture);

                    }
                }

            }
            else                                          // create a new accommodation type
            {
                var accomodation = new Accomodation();

                accomodation.Name = model.Name;
                accomodation.AccomodationPackageId = model.AccomodationPackageId;
                accomodation.Description = model.Description;

                _context.Accomodations.Add(accomodation);

                if (model.PictureFiles[0] != null)
                {
                    foreach (var pictureFile in model.PictureFiles)
                    {
                        string directoryPath = "~/images/UploadedImages/";

                        string fileName = Path.GetFileNameWithoutExtension(pictureFile.FileName);

                        string extension = Path.GetExtension(pictureFile.FileName);
                        fileName = fileName + "-" + DateTime.Now.ToString("yymmssfff") + Guid.NewGuid() + extension;
                        var ServerSavePath = Path.Combine(Server.MapPath(directoryPath) + fileName);

                        //Save file to server folder  
                        pictureFile.SaveAs(ServerSavePath);
                        //assigning file uploaded status to ViewBag for showing message to user.  

                        var picture = new Picture();
                        picture.Url = (string)directoryPath + fileName;
                        picture.AccomodationId = model.Id;
                        _context.Pictures.Add(picture);

                    }
                }
            }

            _context.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var accomodation = _context.Accomodations.Find(id);
            var model = new AccomodationActionViewModel()
            {
                Id = accomodation.Id

            };
            return PartialView("_Delete", model);
        }

        [HttpPost]
        public ActionResult Delete(Accomodation model)
        {

            var accomodation = _context.Accomodations.Find(model.Id);
            var pics = _context.Pictures.Where(p => p.AccomodationId == accomodation.Id).ToList();
            foreach (var pic in pics)
            {
                string fullPath = Request.MapPath("~" + Url.Content(pic.Url));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                _context.Pictures.Remove(pic);
            }

            _context.Accomodations.Remove(accomodation);
            _context.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PictureDelete(int picid)
        {
            var pic = _context.Pictures.Find(picid);
            string fullPath = Request.MapPath("~" + Url.Content(pic.Url));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            _context.Pictures.Remove(pic);
            _context.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}