using System;
using Models;

namespace Models
{
    public class NhanVien
    {
        public string MaNhanVien { get; set; }
        public string HoTen { get; set; }
        public string NgaySinh { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public string ChucVu { get; set; }
        public int SoNamCongTac { get; set; }
        
        

        public bool KiemTraTrungLap(NhanVien nhanVien)
        {
            return this.HoTen == nhanVien.HoTen && this.NgaySinh == nhanVien.NgaySinh;
        }

        
       public NhanVien Clone()
        {
            return (NhanVien)this.MemberwiseClone();
        }
    }
}
namespace DataAccess
{
    
    public class NhanVienDataAccess
    {
        private Random random = new Random();
        //Tạo danh sach nhân viên ngẫu nhiên
        public List<NhanVien> nhanvienngaunhien()
        {
            List<NhanVien> danhSachNhanVien = new List<NhanVien>();
            for (int i = 0; i < 5; i++)
            {
                NhanVien nhanVien = new NhanVien();
                nhanVien.MaNhanVien = "NV-" + (i + 1).ToString("0000"); 
                nhanVien.HoTen = "Nhân viên " + (i + 1);
                nhanVien.NgaySinh = NgaySinh();
                nhanVien.SoDienThoai = SoDienThoai();
                nhanVien.DiaChi = "Địa chỉ nhân viên " + (i + 1);
                nhanVien.ChucVu = "Chức vụ nhân viên " + (i + 1);
                nhanVien.SoNamCongTac = random.Next(1, 10); 

                danhSachNhanVien.Add(nhanVien);
            }
            return danhSachNhanVien;
        }

        //Tạo giới hạn cho ngày sinh 
        private string NgaySinh()
        {
            DateTime ngayBatDau = DateTime.Now.AddYears(-50);
            int soNgay = (DateTime.Now - ngayBatDau).Days;
            int soNgayNgauNhien = random.Next(soNgay);
            DateTime ngaySinh = ngayBatDau.AddDays(soNgayNgauNhien).Date;
            return ngaySinh.ToString("dd/MM/yyyy");
        }

        //đặt quy luật random sdt
        private string SoDienThoai()
        {
            return "0" + random.Next(100000000, 999999999).ToString();
        }

        
    }
}
