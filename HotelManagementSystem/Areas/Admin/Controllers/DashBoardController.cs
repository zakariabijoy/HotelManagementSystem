using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManagementSystem.Areas.Admin.Controllers
{
    public class DashBoardController : Controller
    {
        // GET: Admin/DashBoard
        public ActionResult Index()
        {
            return View();
        }
    }
}