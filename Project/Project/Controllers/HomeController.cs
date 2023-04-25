using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace Project.Controllers
{
    public class HomeController : Controller
    {
        AppleDataDataContext db = new AppleDataDataContext();
        public ActionResult Index()
        {
            ViewBag.iPhone = db.Items.Where(s => s.maloai == 1).Take(4).ToList();
            ViewBag.mac = db.Items.Where(s => s.maloai == 2).Take(4).ToList();

            return View(db.Items.ToList());
        }

        public ActionResult Store()
        {
            ViewBag.iPhone = db.Items.Where(s => s.maloai == 1).Take(4).ToList();
            ViewBag.mac = db.Items.Where(s => s.maloai == 2).Take(4).ToList();
            ViewBag.iPad = db.Items.Where(s => s.maloai == 3).Take(4).ToList();
            ViewBag.Watch = db.Items.Where(s => s.maloai == 4).Take(4).ToList();
            ViewBag.Airpods = db.Items.Where(s => s.maloai == 5).Take(4).ToList();
            return View();
        }    public ActionResult Product_Details(int id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var item = db.Items.FirstOrDefault(d =>d.ma==id);
                ViewBag.HinhAnh = db.HinhAnhs.Where(s => s.ma == id).ToList();
                return View(item);
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

        [ChildActionOnly]
        public ActionResult MenuView()
        {
            var menu = db.Loais.ToList();
            return PartialView(menu);
        }

    }
}