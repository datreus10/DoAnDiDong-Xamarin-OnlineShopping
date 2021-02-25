using DoAnDiDong.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;

using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;


namespace DoAnDiDong.ViewModel
{
    public class RegisterViewModel:BaseViewModel
    {

        public ICommand RegisterCommand { get; private set; }

        private KhachHang kh;

        public KhachHang KH_New
        {
            get { return kh; }
            set
            {
                kh = value;
                RaisePropertyChanged("KH");
            }
        }

        public RegisterViewModel()
        {
            KH_New = new KhachHang();
            KH_New.NgaySinh = DateTime.Now;
            RegisterCommand = new Command(registerCommand);
        }
        private async void registerCommand()
        {
            if (CheckValidInput(KH_New))
            {
                HttpClient http = new HttpClient();
                int kt_gt = KH_New.GioiTinh ? 1 : 0;
                string bday = KH_New.NgaySinh.ToString("yyyy-MM-dd");
                string temp = await http.GetStringAsync
                    ($"http://datreus123.somee.com/api/serviceController/RegisterKH?HoTen={KH_New.HoTen}&DiaChi={KH_New.DiaChi}&DienThoai={KH_New.DienThoai}&TenDangNhap={KH_New.TenDangNhap}&MatKhau={KH_New.MatKhau}&NgaySinh={bday}&GioiTinh={kt_gt}&Email={KH_New.Email}");
                if (temp == "-1")
                {
                    await Shell.Current.DisplayAlert("Lỗi", "Tên đăng nhập đã tồn tại", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Thông báo", "Đăng kí thành công", "OK");
                    //await Shell.Current.Navigation.PopAsync();
                }
            }
        }
        private bool CheckValidInput(KhachHang k)
        {

            //Kiem tra gia tri dau vao khac null hoac rong
            if (string.IsNullOrEmpty(k.HoTen))
            {
                Shell.Current.DisplayAlert("Lỗi", "Họ tên trống", "OK");
                return false;
            }
            if (string.IsNullOrEmpty(k.DiaChi))
            {
                Shell.Current.DisplayAlert("Lỗi", "Địa chỉ trống", "OK");
                return false;
            }
            if (string.IsNullOrEmpty(k.DienThoai))
            {
                Shell.Current.DisplayAlert("Lỗi", "Điện thoại trống", "OK");
                return false;
            }
            if (string.IsNullOrEmpty(k.TenDangNhap))
            {
                Shell.Current.DisplayAlert("Lỗi", "Tên đăng nhập trống", "OK");
                return false;
            }
            if (string.IsNullOrEmpty(k.MatKhau))
            {
                Shell.Current.DisplayAlert("Lỗi", "Mật khẩu trống", "OK");
                return false;
            }
            if (string.IsNullOrEmpty(k.Email))
            {
                Shell.Current.DisplayAlert("Lỗi", "Email trống", "OK");
                return false;
            }

            //so dien thoai chi chua so va co 10 so
            if (!k.DienThoai.All(char.IsDigit) || k.DienThoai.Length != 10)
            {
                //k.DienThoai = "";
                Shell.Current.DisplayAlert("Lỗi", "Số điện thoại không hợp lệ", "OK");
                return false;
            }

            //mat khau chi chua chu va so
            if (k.MatKhau.Any(ch => !Char.IsLetterOrDigit(ch)))
            {
                //k.MatKhau = "";
                Shell.Current.DisplayAlert("Lỗi", "Mật khẩu chỉ được chứa chữ và số", "OK");
                return false;
            }

            //kiem tra email don gian (ko dc chua khoang trang va email phai chua @ va . )
            Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            bool notValidEmail = string.IsNullOrWhiteSpace(KH_New.Email) || !EmailRegex.IsMatch(KH_New.Email);
            if (notValidEmail)
            {
                //k.Email = "";
                Shell.Current.DisplayAlert("Lỗi", "Email không hợp lệ", "OK");
                return false;
            }
            return true;

        }
    }
}
