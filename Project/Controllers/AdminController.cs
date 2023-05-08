using Project.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public ActionResult ThongKeDoanhThu(DateTime? date)
        {
            // Truy vấn danh sách đơn hàng và chi tiết đơn hàng
            var dhList = db.DonHangs.ToList();
            var ctdhList = db.ChiTietDonHangs.ToList();

            // Tạo danh sách doanh thu
            var revenueList = (from ctdh in ctdhList
                               join dh in dhList on ctdh.madon equals dh.madon
                               where dh.ngaygiao != null // chỉ lấy các đơn hàng có ngày giao
                               group ctdh by dh.ngaygiao into g
                               select new RevenueStatistics
                               {
                                   NgayGiao = (DateTime)g.Key,
                                   TongTien = (decimal)g.Sum(ctdh => ctdh.tongtien)
                               }).ToList();

            // Sắp xếp danh sách theo ngày
            revenueList.Sort((x, y) => x.NgayGiao.CompareTo(y.NgayGiao));

            // Lọc danh sách đơn hàng nếu người dùng chọn ngày
            DateTime selectedDate = date ?? DateTime.Today;
            dhList = dhList.Where(dh => dh.ngaygiao == selectedDate).ToList();

            // Tính toán doanh thu cho danh sách đơn hàng
            var selectedDateRevenue = (from ctdh in ctdhList
                                       join dh in dhList on ctdh.madon equals dh.madon
                                       group ctdh by dh.ngaygiao into g
                                       select new RevenueStatistics
                                       {
                                           NgayGiao = (DateTime)g.Key,
                                           TongTien = (decimal)g.Sum(ctdh => ctdh.tongtien)
                                       }).FirstOrDefault();

            // Trả về view hiển thị kết quả
            ViewBag.RevenueStatistics = revenueList;
            ViewBag.SelectedDate = date;
            ViewBag.SelectedDateRevenue = selectedDateRevenue;

            return View();
        }
    }
}