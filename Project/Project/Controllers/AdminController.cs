using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
      AppleDataDataContext db = new AppleDataDataContext();
        public ActionResult Index()
        {
            var all_user = from s in db.KhachHangs select s;
            ViewBag.User = all_user.ToList();
            var result = (from nv in db.NhanViens
                          join cv in db.ChucVus on nv.MaCV equals cv.MaCV
                          select new Staff_Info { TenCV=cv.TenCV,Ten= nv.Ten,Ho= nv.Ho }).ToList();
            ViewBag.Staff = result;
            return View();
        }
        public ActionResult Add_Products()
        {
            return View();
        }
        public ActionResult ThongKe()
        {
            return View();
        }
    }
}