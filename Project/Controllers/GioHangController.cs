
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppleZone.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        AppleDataDataContext data = new AppleDataDataContext();
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["GioHang"] = lstGiohang;

            }
            return lstGiohang;
        }

        public ActionResult ThemGioHang(int id, string strURL)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.Find(n => n.ma == id);

            if (sanpham == null)
            {
                sanpham = new Giohang(id);
                lstGiohang.Add(sanpham);
                if (lstGiohang.Count == 1)
                {
                    return RedirectToAction("GioHang");
                }
                else
                {
                    TempData["ThongBaoGioHang"] = "Đã thêm sản phẩm vào giỏ hàng";
                    return Redirect(strURL);
                }
            }

            else
            {
                TempData["ThongBaoGioHang"] = "Đã thêm sản phẩm vào giỏ hàng";
                sanpham.isoLuong++;
                return Redirect(strURL);
            }
        }

        private int TongSoLuong()
        {
            int tsl = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Sum(n => n.isoLuong);

            }
            return tsl;
        }
        public int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Count;
            }
            return tsl;

        }
        private double TongTien()
        {
            double tt = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                tt = lstGiohang.Sum(n => n.dthanhtien);

            }
            return tt;
        }


        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("GioHangTrong");

            }
            else
            {
                ViewBag.TongSoLuong = TongSoLuong();
                ViewBag.Tongtien = TongTien();
                ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
                return View(lstGiohang);
            }

        }
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return PartialView();
        }
        public ActionResult XoaGiohang(int id)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.ma == id);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.ma == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapnhatGioHang(int id, FormCollection collection)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.ma == id);
            if (sanpham != null)
            {
                sanpham.isoLuong = int.Parse(collection["txtSolg"].ToString());
            }
            return RedirectToAction("GioHang");

        }
        public ActionResult XoaTatCaGioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("GioHang");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("LogIn", "Users");
            }
            List<Giohang> listGioHang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(listGioHang);
        }

        public ActionResult DatHang(FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            Item s = new Item();
            List<Giohang> listGioHang = Laygiohang();
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
            dh.makh = kh.makh;
            dh.ngaydat = DateTime.Now;
            dh.ngaygiao = DateTime.Parse(ngaygiao);
            dh.giaohang = false;
            dh.thanhtoan = false;
            data.DonHangs.InsertOnSubmit(dh);
            data.SubmitChanges();
            foreach (var item in listGioHang)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.madon = dh.madon;
                ctdh.ma = item.ma;
                ctdh.soluong = item.isoLuong;
                ctdh.gia = (decimal)item.giaban;
                s = data.Items.Single(n => n.ma == item.ma);
                s.soluongton -= ctdh.soluong;
                data.SubmitChanges();
                data.ChiTietDonHangs.InsertOnSubmit(ctdh);


            }
            //foreach (var item in listGioHang)
            //{
            //    GioHang gh = new GioHang();
            //    gh.ten = item.ten;
            //    gh.hinh = item.hinh;
            //    gh.giaban = (decimal)(item.giaban);
            //    gh.soluong = item.isoLuong;
            //    s = data.Items.Single(n => n.ma == item.ma);
            //    s.soluongton -= gh.soluong;
            //    gh.dthanhtien = (decimal)(item.isoLuong * item.giaban);
            //    data.SubmitChanges();
            //    data.GioHangs.InsertOnSubmit(gh);
            //}


            data.SubmitChanges();

            Session["GioHang"] = null;

            return RedirectToAction("XacnhanDonHang", "GioHang");
        }

        public ActionResult XacnhanDonHang()
        {
            return View();
        }
        public ActionResult GioHangTrong()
        {
            return View();
        }

    }
}