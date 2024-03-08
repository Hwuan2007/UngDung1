using System;
using System.Collections.Generic;
using Models;
using DataAccess;
using System.Text;
using System.Globalization;
using System.IO;
using System.Text.Json;


namespace ung_dung
{
    public class ung_dung_1
    {
        static List<NhanVien> danhSachNhanVien;
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            
            bool running = true;
            NhanVienDataAccess dataAccess = new NhanVienDataAccess();
            danhSachNhanVien = dataAccess.nhanvienngaunhien();
            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("+---------------MENU---------------+");
                Console.WriteLine("|                                  |");
                Console.WriteLine("|  1.Hiển thị danh sách nhân viên  |");
                Console.WriteLine("|  2.Thêm mới nhân viên            |");
                Console.WriteLine("|  3.Sửa thông tin nhân viên       |");
                Console.WriteLine("|  4.Xóa nhân viên                 |");
                Console.WriteLine("|  5. Tìm kiếm                     |");
                Console.WriteLine("|  6. Lưu ra File                  |");
                Console.WriteLine("|  0.Thoát                         |");
                Console.WriteLine("|                                  |");
                Console.WriteLine("+----------------------------------+");
                Console.WriteLine("Hãy nhập lựa chọn của bạn: ");
                int choose = int.Parse(Console.ReadLine());
                
                switch (choose)
                {
                    case 1:
                        Console.WriteLine("Bạn đã chọn 'Hiển thị danh sách nhân viên'");
                        HienThiDanhSachNhanVien();
                        EscapeButton();
                        break;
                    case 2:
                        Console.WriteLine("Bạn đã chọn 'Thêm mới nhân viên'");
                        ThemNhanVienMoi();
                        EscapeButton();
                        break;
                    case 3:
                        Console.WriteLine("Bạn đã chọn 'Sửa thông tin nhân viên'");
                        SuaThongTinNhanVien();
                        EscapeButton();
                        break;
                    case 4:
                        Console.WriteLine("Bạn đã chọn 'Xóa nhân viên'");
                        XoaNhanVien();
                        EscapeButton();
                        break;
                    case 5:
                    Console.WriteLine("Bạn đã chọn 'Tìm kiếm'");
                    TimKiemNhanVien();
                    EscapeButton();
                    break;
                    case 6:
                    Console.WriteLine("Bạn đã chọn 'Lưu ra File'");
                    LuuDanhSachNhanVien();
                    EscapeButton();
                    break;
                    case 0:
                        Console.WriteLine("Bạn đã chọn 'Thoát'");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Lựa chọn của bạn không phù hợp"); 
                        break;
                }
            }
        }

        // Hiển thị danh sách nhân viên
        static void HienThiDanhSachNhanVien()
        
        {
            if (danhSachNhanVien == null || danhSachNhanVien.Count == 0)
            {
                Console.WriteLine("Danh sách nhân viên trống.");
                return;
            }

            Console.WriteLine("+--------+---------------------+---------------------+---------------+-------------------------+---------------------+----------------+");
            Console.WriteLine("| Mã NV  |      Họ và Tên      | Ngày Tháng Năm Sinh | Số Điện Thoại |         Địa Chỉ         |       Chức vụ       | Số năm công tác|");
            Console.WriteLine("+--------+---------------------+---------------------+---------------+-------------------------+---------------------+----------------+");

            foreach (var nhanVien in danhSachNhanVien)
            {
            Console.WriteLine($"| {nhanVien.MaNhanVien,-7}| {nhanVien.HoTen,-20}| {nhanVien.NgaySinh,-20}| {nhanVien.SoDienThoai,-14}| {nhanVien.DiaChi,-24}| {nhanVien.ChucVu,-20}| {nhanVien.SoNamCongTac+" năm", -15}|");
            }

            Console.WriteLine("+--------+---------------------+---------------------+---------------+-------------------------+---------------------+----------------+");


        }

        //Thêm nhân viên mới
        static void ThemNhanVienMoi()
        {
            NhanVien nhanVienMoi = new NhanVien();
            nhanVienMoi.MaNhanVien = UpdateMaNhanVien();

            Console.WriteLine("Nhập thông tin của nhân viên mới:");
            string hoTen;
            do
            {
                Console.Write("Họ và Tên: ");
                hoTen = Console.ReadLine(); 
                if (string.IsNullOrEmpty(hoTen))
                {
                    Console.WriteLine("Tên không được để trống. Vui lòng nhập lại.");
                }
                else if (danhSachNhanVien.Exists(x => x.HoTen == hoTen))
                {
                    Console.WriteLine("Nhân viên có tên này đã tồn tại trong danh sách. Vui lòng nhập lại.");
                }
                else
                {
                    break;
                }
            } while (true);
            nhanVienMoi.HoTen = hoTen;

            string ngaySinhStr;
            do
            {
                Console.Write("Ngày Sinh (dd/MM/yyyy): ");
                ngaySinhStr = Console.ReadLine();

                if (string.IsNullOrEmpty(ngaySinhStr))
                {
                    Console.WriteLine("Ngày sinh không được để trống. Vui lòng nhập lại.");
                }
                else if (!DateTime.TryParseExact(ngaySinhStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    Console.WriteLine("Ngày sinh không hợp lệ. Vui lòng nhập lại theo định dạng dd/MM/yyyy.");
                }
                else if (danhSachNhanVien.Exists(x => x.NgaySinh == ngaySinhStr))
                {
                    Console.WriteLine("Nhân viên có ngày sinh này đã tồn tại trong danh sách. Vui lòng nhập lại.");
                }
                else
                {
                    break;
                }
            } while (true);
            nhanVienMoi.NgaySinh = ngaySinhStr;
            
            string soDienThoai;
            do
            {
                Console.Write("Số Điện Thoại: ");
                soDienThoai = Console.ReadLine();

                if (string.IsNullOrEmpty(soDienThoai))
                {
                    Console.WriteLine("Số điện thoại không được để trống. Vui lòng nhập lại.");
                }
                else if (!IsValidPhoneNumber(soDienThoai))
                {
                    Console.WriteLine("Số điện thoại không hợp lệ. Vui lòng nhập lại.");
                }
                else
                {
                    break; 
                }
            } while (true);
            nhanVienMoi.SoDienThoai = soDienThoai;

            string diaChi;
            do
            {
                Console.Write("Địa Chỉ: ");
                diaChi = Console.ReadLine(); 
                if (string.IsNullOrEmpty(diaChi))
                {
                    Console.WriteLine("Địa chỉ không được để trống. Vui lòng nhập lại.");
                }
                else
                {
                    break;
                }
            } while (true);
            nhanVienMoi.DiaChi = diaChi;

            string chucvu;
            do
            {
                Console.Write("Chức vụ: ");
                chucvu = Console.ReadLine(); 
                if (string.IsNullOrEmpty(chucvu))
                {
                    Console.WriteLine("Chức vụ không được để trống. Vui lòng nhập lại.");
                }
                else
                {
                    break;
                }
            } while (true);
            nhanVienMoi.ChucVu = chucvu;

            int soNamCongTac;
            do
            {
                Console.Write("Số Năm Công Tác: ");
                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Số năm công tác không được để trống. Vui lòng nhập lại.");
                }
                else
                {
                    soNamCongTac = int.Parse(input);
                    break;
                }
            } while (true);
            nhanVienMoi.SoNamCongTac = soNamCongTac;


            danhSachNhanVien.Add(nhanVienMoi);

            Console.WriteLine("Nhân viên mới đã được thêm vào danh sách.");
        }

        //Sửa thông tin nhân viên
        static void SuaThongTinNhanVien()
        {
            Console.Write("Nhập mã nhân viên cần chỉnh sửa: ");
            string maNhanVienSua = Console.ReadLine();

            NhanVien nhanVienCanSua = danhSachNhanVien.FirstOrDefault(nv => nv.MaNhanVien == maNhanVienSua);

            if (nhanVienCanSua != null)
            {

                Console.WriteLine($"Thông tin nhân viên của mã nhân viên {nhanVienCanSua.MaNhanVien} cần chỉnh sửa:");
                Console.WriteLine($"1. Họ và tên: {nhanVienCanSua.HoTen}");
                Console.WriteLine($"2. Ngày sinh: {nhanVienCanSua.NgaySinh}");
                Console.WriteLine($"3. Số điện thoại: {nhanVienCanSua.SoDienThoai}");
                Console.WriteLine($"4. Địa chỉ: {nhanVienCanSua.DiaChi}");
                Console.WriteLine($"5. Chức vụ: {nhanVienCanSua.ChucVu}");
                Console.WriteLine($"6. Số năm công tác: {nhanVienCanSua.SoNamCongTac}");

                Console.Write("Nhập số thứ tự của thông tin cần chỉnh sửa: ");
                int Option = int.Parse(Console.ReadLine());

                string newValue;
                switch (Option)
                {
                    case 1:
                    do
                    {
                        Console.Write("Nhập họ và tên mới: ");
                        newValue = Console.ReadLine();
                        if (string.IsNullOrEmpty(newValue))
                        {
                            Console.WriteLine("Tên không được để trống. Vui lòng nhập lại.");
                        }
                        else if (danhSachNhanVien.Exists(x => x.HoTen == newValue))
                        {
                            Console.WriteLine("Nhân viên có tên này đã tồn tại trong danh sách. Vui lòng nhập lại.");
                        }
                    } while (string.IsNullOrEmpty(newValue));
                    var editedNhanVien1 = nhanVienCanSua.Clone();
                    editedNhanVien1.HoTen = newValue;
                    ShowEditedInfo(editedNhanVien1, maNhanVienSua);
                    break;
                case 2:
                    do
                    {
                        Console.Write("Nhập ngày sinh mới (dd/MM/yyyy): ");
                        newValue = Console.ReadLine();
                         if (string.IsNullOrEmpty(newValue))
                        {
                            Console.WriteLine("Ngày sinh không được để trống. Vui lòng nhập lại.");
                        }
                        else if (!DateTime.TryParseExact(newValue, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                        {
                            Console.WriteLine("Ngày sinh không hợp lệ. Vui lòng nhập lại theo định dạng dd/MM/yyyy.");
                        }
                        else if (danhSachNhanVien.Exists(x => x.NgaySinh == newValue))
                        {
                            Console.WriteLine("Nhân viên có ngày sinh này đã tồn tại trong danh sách. Vui lòng nhập lại.");
                        }
                    } while (string.IsNullOrEmpty(newValue) || !DateTime.TryParseExact(newValue, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
                    var editedNhanVien2 = nhanVienCanSua.Clone();
                    editedNhanVien2.NgaySinh = newValue;
                    ShowEditedInfo(editedNhanVien2, maNhanVienSua);
                    break;
                case 3:
                    do
                    {
                        Console.Write("Nhập số điện thoại mới: ");
                        newValue = Console.ReadLine();
                        if (string.IsNullOrEmpty(newValue))
                        {
                            Console.WriteLine("Số điện thoại không được để trống. Vui lòng nhập lại.");
                        }
                        else if (!IsValidPhoneNumber(newValue))
                        {
                            Console.WriteLine("Số điện thoại không hợp lệ. Vui lòng nhập lại.");
                        }
                    } while (string.IsNullOrEmpty(newValue) || !IsValidPhoneNumber(newValue));
                    var editedNhanVien3 = nhanVienCanSua.Clone();
                    editedNhanVien3.SoDienThoai = newValue;
                    ShowEditedInfo(editedNhanVien3, maNhanVienSua);
                    break;
                case 4:
                    do
                    {
                        Console.Write("Nhập địa chỉ mới: ");
                        newValue = Console.ReadLine();
                        if (string.IsNullOrEmpty(newValue))
                        {
                            Console.WriteLine("Địa chỉ không được để trống.");
                        }
                    } while (string.IsNullOrEmpty(newValue));
                    var editedNhanVien4 = nhanVienCanSua.Clone();
                    editedNhanVien4.DiaChi = newValue;
                    ShowEditedInfo(editedNhanVien4, maNhanVienSua);
                    break;
                case 5:
                    do
                    {
                        Console.Write("Nhập chức vụ mới: ");
                        newValue = Console.ReadLine();
                        if (string.IsNullOrEmpty(newValue))
                        {
                            Console.WriteLine("Chức vụ không được để trống.");
                        }
                    } while (string.IsNullOrEmpty(newValue));
                    var editedNhanVien5 = nhanVienCanSua.Clone();
                    editedNhanVien5.ChucVu = newValue;
                    ShowEditedInfo(editedNhanVien5, maNhanVienSua);
                    break;
                case 6:
                    int newSoNamCongTac;
                    do
                    {
                        Console.Write("Nhập số năm công tác mới: ");
                        newValue = Console.ReadLine();
                        if (string.IsNullOrEmpty(newValue))
                        {
                            Console.WriteLine("Số năm công tác không được để trống.");
                        }
                        else if (!int.TryParse(newValue, out newSoNamCongTac))
                        {
                            Console.WriteLine("Số năm công tác không hợp lệ.");
                        }
                    } while (string.IsNullOrEmpty(newValue) || !int.TryParse(newValue, out newSoNamCongTac));
                    var editedNhanVien6 = nhanVienCanSua.Clone();
                    editedNhanVien6.SoNamCongTac = newSoNamCongTac;
                    ShowEditedInfo(editedNhanVien6, maNhanVienSua);
                    break;
                default:
                    Console.WriteLine("Lựa chọn không hợp lệ.");
                    break;
                }
            }
            else
            {
                Console.WriteLine("Không tìm thấy nhân viên có mã vừa nhập.");
            }
        }
        //Hiển thị thông tin mới sửa
        static void ShowEditedInfo(NhanVien editedNhanVien, string maNhanVienSua)
        {
            Console.WriteLine($"Thông tin mới của nhân viên có mã {maNhanVienSua} sau khi chỉnh sửa:");
            Console.WriteLine($"1. Họ và tên: {editedNhanVien.HoTen}");
            Console.WriteLine($"2. Ngày sinh: {editedNhanVien.NgaySinh}");
            Console.WriteLine($"3. Số điện thoại: {editedNhanVien.SoDienThoai}");
            Console.WriteLine($"4. Địa chỉ: {editedNhanVien.DiaChi}");
            Console.WriteLine($"5. Chức vụ: {editedNhanVien.ChucVu}");
            Console.WriteLine($"6. Số năm công tác: {editedNhanVien.SoNamCongTac}");

            Console.WriteLine("Nhấn Enter để lưu dữ liệu đã sửa hoặc nhấn phím khác để bỏ qua.");
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                int index = danhSachNhanVien.FindIndex(nv => nv.MaNhanVien == maNhanVienSua);
                danhSachNhanVien[index] = editedNhanVien;
                Console.WriteLine("Thông tin đã được lưu.");
            }
            else
            {
                Console.WriteLine("Thông tin không được lưu.");
            }
        }
        //xóa nhân viên
        static void XoaNhanVien(){
            Console.Write("Nhập mã nhân viên cần chỉnh sửa: ");
            string maNhanVienXoa = Console.ReadLine();

            NhanVien nhanVienCanXoa = danhSachNhanVien.FirstOrDefault(nv => nv.MaNhanVien == maNhanVienXoa);


            if (nhanVienCanXoa != null)
            {

                Console.WriteLine($"Thông tin nhân viên của mã nhân viên {nhanVienCanXoa.MaNhanVien} cần chỉnh sửa:");
                Console.WriteLine($"1. Họ và tên: {nhanVienCanXoa.HoTen}");
                Console.WriteLine($"2. Ngày sinh: {nhanVienCanXoa.NgaySinh}");
                Console.WriteLine($"3. Số điện thoại: {nhanVienCanXoa.SoDienThoai}");
                Console.WriteLine($"4. Địa chỉ: {nhanVienCanXoa.DiaChi}");
                Console.WriteLine($"5. Chức vụ: {nhanVienCanXoa.ChucVu}");
                Console.WriteLine($"6. Số năm công tác: {nhanVienCanXoa.SoNamCongTac}");   
                
                Console.Write("Bạn có chắc chắn muốn xóa nhân viên này? (y/n): ");
                string confirmation = Console.ReadLine().ToLower();

                if (confirmation == "y")
                {
                    danhSachNhanVien.Remove(nhanVienCanXoa);
                    Console.WriteLine("Nhân viên đã được xóa.");
                }
                else if (confirmation == "n")
                {
                    Console.WriteLine("Hủy thao tác xóa nhân viên.");
                }
                else
                {
                    Console.WriteLine("Lựa chọn không hợp lệ.");
                }     
            }
            else
            {
                Console.WriteLine("Không tìm thấy nhân viên có mã vừa nhập.");
            }
        }

        //Tìm kiếm nhân viên
        static void TimKiemNhanVien()
        {
            Console.Write("Nhập từ khóa tìm kiếm: ");
            string keyword = Console.ReadLine().ToLower();

            List<NhanVien> result = danhSachNhanVien.FindAll(key => key.HoTen.Contains(keyword) || key.DiaChi.Contains(keyword));

            if (result.Count > 0)
            {
                Console.WriteLine("Kết quả tìm kiếm:");
                Console.WriteLine("+--------+---------------------+---------------------+---------------+-------------------------+---------------------+----------------+");
                Console.WriteLine("| Mã NV  |      Họ và Tên      | Ngày Tháng Năm Sinh | Số Điện Thoại |         Địa Chỉ         |       Chức vụ       | Số năm công tác|");
                Console.WriteLine("+--------+---------------------+---------------------+---------------+-------------------------+---------------------+----------------+");

                foreach (var nhanVien in result)
                {
                    Console.WriteLine($"| {nhanVien.MaNhanVien,-7}| {nhanVien.HoTen,-20}| {nhanVien.NgaySinh,-20}| {nhanVien.SoDienThoai,-14}| {nhanVien.DiaChi,-24}| {nhanVien.ChucVu,-20}| {nhanVien.SoNamCongTac + " năm", -15}|");
                }

                Console.WriteLine("+--------+---------------------+---------------------+---------------+-------------------------+---------------------+----------------+");
            }
            else
            {
                Console.WriteLine("Không tìm thấy nhân viên nào phù hợp với từ khóa tìm kiếm.");
            }
        }
        //Lưu file Json
        static void LuuDanhSachNhanVien()
        {
            if (danhSachNhanVien == null || danhSachNhanVien.Count == 0)
            {
                Console.WriteLine("Không có dữ liệu để lưu.");
                return;
            }

            Console.Write("Nhập tên file để lưu (bao gồm cả đuôi .json): ");
            string fileName = Console.ReadLine();

            try
            {
                using (FileStream fs = File.Create(fileName))
                {
                    JsonSerializer.SerializeAsync(fs, danhSachNhanVien);
                    Console.WriteLine("Danh sách nhân viên đã được lưu thành công.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã xảy ra lỗi khi lưu file: {ex.Message}");
            }
        }
        //tạo quy luật nhập sdt
        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            foreach (char c in phoneNumber)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            if (phoneNumber.Length != 10 && phoneNumber.Length != 11)
            {
                return false;
            }

            return true;
        }
        //tạo id nhận viên tự động
        static string UpdateMaNhanVien()
        {
            Random random = new Random();
            while (true)
            {
                int randomNumber = random.Next(1, 10000);
                string maNhanVien = "NV-" + randomNumber.ToString("0000");
                if (!danhSachNhanVien.Any(nv => nv.MaNhanVien == maNhanVien))
                {
                    return maNhanVien; 
                }
            }
        }
        //Nút thoát tác vụ
        static void EscapeButton()
        {
            Console.WriteLine("Nhấn phím Esc để quay lại menu chính.");
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Escape)
                        return;
                }
            }
        }
        
        
   
    }
    
}
