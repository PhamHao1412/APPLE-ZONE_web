﻿
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace AppleZone.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        AppleDataDataContext data = new AppleDataDataContext();
        
        private int CountProductIDs(int makh)
        {
            int count = 0;
            var gioHangList = data.GioHangs;
            var idList = gioHangList.Where(g => g.makh == makh).Select(g => g.masp).ToList();
            count = idList.Distinct().Count();
            return count;
        }
        private decimal Total(int makh)
        {
            decimal total = 0;
            var giohang = data.GioHangs.Where(g => g.makh == makh);
           if(giohang != null)
            {
                total = (giohang?.Sum(g => g.tongtien) ?? 0);
            }
            return total;
        }
        public ActionResult ThemGioHang(int id)
        {
            
            var item = data.Items.FirstOrDefault(m => m.ma == id);
            KhachHang kh = (KhachHang)Session["TaiKhoan"];

            if (item != null)
            {
                GioHang gh = data.GioHangs.FirstOrDefault(g => g.masp == id && g.makh==kh.makh);

                if (gh != null)
                {
                    gh.soluong += 1;
                    gh.tongtien = gh.soluong * item.giaban;
                }
                else
                {
                    gh = new GioHang
                    {
                        masp = id,
                        makh = kh.makh,
                        ten = item.ten,
                        hinh = item.hinh,
                        giaban = item.giaban,
                        soluong = 1,
                        tongtien = item.giaban
                    };

                    data.GioHangs.InsertOnSubmit(gh);
                }
                item.soluongton -= 1;
                data.SubmitChanges();
            }

            return RedirectToAction("Store", "Home");
        }
     

        public ActionResult GioHang()
        {
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            if(kh == null)
            {
                return RedirectToAction("GioHang", "GioHang");

            }
            ViewBag.ShoppingCart = data.GioHangs.Where(g => g.makh == kh.makh).ToList();
            ViewBag.listGioHang = data.GioHangs.ToList();
            ViewBag.CountCart = CountProductIDs(kh.makh);
            ViewBag.TotalPrice = Total(kh.makh);
            return View();
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            ViewBag.TotalPrice = Total(kh.makh);
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("LogIn", "Users");
            }
            return View();
        }
        public ActionResult DatHang(FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            Item s = new Item();
            string text = "Đơn hàng mới";
            var listGioHang = data.GioHangs.Where(g => g.makh == kh.makh).ToList();
            dh.makh = kh.makh;
            dh.ngaydat = DateTime.Now;
            dh.ngaygiao = DateTime.Now;
            dh.giaohang = false;
            dh.thanhtoan = false;
            dh.trangthai = text;
            data.DonHangs.InsertOnSubmit(dh);
            data.SubmitChanges();
            foreach (var item in listGioHang)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.madon = dh.madon;
                ctdh.ma =(int) item.masp;
                ctdh.soluong = item.soluong;
                ctdh.gia = (decimal)item.giaban;
                s = data.Items.Single(n => n.ma == item.masp);
                s.soluongton -= ctdh.soluong;
                ctdh.tongtien = (decimal)(item.soluong * item.giaban);
                data.SubmitChanges();
                data.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            data.GioHangs.DeleteAllOnSubmit(listGioHang);
            data.SubmitChanges();
            return RedirectToAction("XacnhanDonHang", "GioHang");
        }
        public ActionResult XoaGioHang(int masp)
        {
            GioHang gh = data.GioHangs.FirstOrDefault(g => g.masp == masp);
            if (gh != null)
            {
                data.GioHangs.DeleteOnSubmit(gh);
                data.SubmitChanges();
            }

            return RedirectToAction("GioHang", "GioHang");
        }

        [HttpPost]
        public JsonResult CapNhatGioHang(int id, int quantity)
        {
            using (var db = new AppleDataDataContext())
            {
                var cartItems = db.GioHangs.ToList();
                foreach (var cartItem in cartItems)
                {
                    if (cartItem.masp == id)
                    {
                        cartItem.soluong = quantity;
                        cartItem.tongtien = cartItem.soluong * cartItem.giaban;

                    }
                }
                db.SubmitChanges(); 
                return Json(new { success = true });
            }
        }

        public ActionResult XacnhanDonHang()
        {
            return View();
        }


    }
}