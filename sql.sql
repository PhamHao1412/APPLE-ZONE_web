use Apple
ALTER TABLE GioHang 
ADD maloai INT FOREIGN KEY REFERENCES Loai(maloai);


select c.madon,c.ma,i.ten,soluong,ngaydat,giaban,tongtien
from  ChiTietDonHang c, Items i,DonHang d,KhachHang k
where c.ma= i.ma and d.madon=c.madon and d.makh=k.makh and k.makh = 5
select * from GioHang


select d.madon,ngaydat,trangthai,ho,k.Ten,ngaysinh,email,dienthoai,diachi,SUM(c.tongtien) AS tongtien
from DonHang d, KhachHang k , ChiTietDonHang c
where d.madon=c.madon and k.makh = d.makh and d.madon = 3051
group by d.madon,ngaydat,trangthai,ho,k.Ten,ngaysinh,email,dienthoai,diachi

