using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace Project.Models
{
    public class Store_Category
    {
        public int MaSP { get; set; }   
        public int MaLoai { get; set; }
        public string TenSP { get; set; }
        public string Hinh { get; set; }
        public string DanhMuc { get; set; }
        public decimal GiaBan { get; set; }
        public decimal GiamGia { get; set; }
        public int SoLuongLike { get; set; }
    }
}