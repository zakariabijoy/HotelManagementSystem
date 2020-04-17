using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelManagementSystem.Areas.Admin.ViewModel;
using HotelManagementSystem.Models;
using PagedList;

namespace HotelManagementSystem.Areas.Admin.Controllers
{
    public class AccomodationPackagesController : Controller
    {
        // GET: Admin/AccomodationPackages
        ApplicationDbContext _context = new ApplicationDbContext();
        // GET: Admin/AccomodationTypes
        public ActionResult Index(string searchTerm, int? accomodationTypeId, int? page)
        {
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
                var pictures    = _context.Pictures.Where(p => p.AccomodationPackageId == accomodationPackage.Id).ToList();
                var model = new AccomodationPackagesActionViewModel()
                {
                    Id = accomodationPackage.Id,
                    Name = accomodationPackage.Name,
                    AccomodationTypeId = accomodationPackage.AccomodationTypeId,
                    NoOfRoom = accomodationPackage.NoOfRoom,
                    FeePerNight = accomodationPackage.FeePerNight,
                    AccomodationTypes = accomodationTypes,
                    Pictures = pictures
                };
                return PartialView("_Action", model);
            }

        }

        [HttpPost]
        public ActionResult Action(AccomodationPackagesActionViewModel model)
        {
            if (model.Id > 0)                           // edit a accommodation type
            {
                var accomodationPackage = _context.AccomodationPackages.Find(model.Id);

                accomodationPackage.Name = model.Name;
                accomodationPackage.AccomodationTypeId = model.AccomodationTypeId;
                accomodationPackage.FeePerNight = model.FeePerNight;
                accomodationPackage.NoOfRoom = model.NoOfRoom;

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
                        picture.AccomodationPackageId = model.Id;
                        _context.Pictures.Add(picture);

                    }
                }
            }
            else                                          // create a new accommodation type
            {
                var accomodationPackage = new AccomodationPackage();

                accomodationPackage.Name = model.Name;
                accomodationPackage.AccomodationTypeId = model.AccomodationTypeId;
                accomodationPackage.FeePerNight = model.FeePerNight;
                accomodationPackage.NoOfRoom = model.NoOfRoom;

                _context.AccomodationPackages.Add(accomodationPackage);

                if (model.PictureFiles[0] != null )
                {
                    foreach (var pictureFile in model.PictureFiles)
                    {
                        string directoryPath = "~/images/UploadedImages/";

                        string fileName = Path.GetFileNameWithoutExtension(pictureFile.FileName);

                        string extension = Path.GetExtension(pictureFile.FileName);
                        fileName = fileName +"-" +DateTime.Now.ToString("yymmssfff") +Guid.NewGuid()+ extension;
                        var ServerSavePath = Path.Combine(Server.MapPath(directoryPath) + fileName);

                        //Save file to server folder  
                        pictureFile.SaveAs(ServerSavePath);
                        //assigning file uploaded status to ViewBag for showing message to user.  

                        var picture = new Picture();
                        picture.Url = (string)directoryPath + fileName;
                        picture.AccomodationPackageId = model.Id;
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
            var pics = _context.Pictures.Where(p => p.AccomodationPackageId == accomodationPackage.Id).ToList();
            foreach (var pic in pics)
            {
                string fullPath = Request.MapPath("~" + Url.Content(pic.Url));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                _context.Pictures.Remove(pic);
            }
            
            _context.AccomodationPackages.Remove(accomodationPackage);
            _context.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PictureDelete( int picid)
        {
            var pic = _context.Pictures.Find(picid);
            string fullPath = Request.MapPath("~" + Url.Content(pic.Url));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            _context.Pictures.Remove(pic);
            _context.SaveChanges();
            return Json(new {success = true}, JsonRequestBehavior.AllowGet);

        }
    }
}