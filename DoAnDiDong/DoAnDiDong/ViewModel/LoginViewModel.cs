using DoAnDiDong.Model;
using DoAnDiDong.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DoAnDiDong.ViewModel
{
    class LoginViewModel : BaseViewModel
    {
        public ICommand LoginCommand { get; private set; }
        public ICommand RegisterCommand { get; private set; }


        private KhachHang kh;
        public KhachHang KH
        {
            get { return kh; }
            set
            {
                kh = value;
                RaisePropertyChanged("KH");
            }
        }

        public LoginViewModel()
        {
            KH = new KhachHang();
            LoginCommand = new Command(loginCommand);
            RegisterCommand = new Command(registerCommand);
        }


        private async void loginCommand()
        {
            if (string.IsNullOrEmpty(KH.TenDangNhap) || string.IsNullOrEmpty(KH.MatKhau))
                await Shell.Current.DisplayAlert("LỖI", "Tên tài khoản hoặc mật khẩu trống", "OK");
            else
            {
                HttpClient http = new HttpClient();
                string temp = await http.GetStringAsync
                    ($"http://datreus123.somee.com/api/serviceController/Login?TenDangNhap={KH.TenDangNhap}&MatKhau={KH.MatKhau}");
                if (temp == "-1")
                {
                    await Shell.Current.DisplayAlert("Lỗi", "Tên tài khoản hoặc mật không đúng", "OK");
                }
                else
                {         
                    temp = await http.GetStringAsync("http://datreus123.somee.com/api/serviceController/GetKH?makh=" + temp);
                    KH = JsonConvert.DeserializeObject<List<KhachHang>>(temp)[0];
                    userID = KH.MaKH;
                    await Shell.Current.DisplayAlert("Thông báo", "Đăng nhập thành công", "OK");
                    //await Shell.Current.GoToAsync("//cart");
                    //await Shell.Current.GoToAsync("//account");
                    MessagingCenter.Send<LoginViewModel, KhachHang>(this, "logined", KH);
                    await Shell.Current.Navigation.PopAsync();
                }
            }
        }

        private void registerCommand()
        {
            Shell.Current.Navigation.PushAsync(new RegisterPage());
        }
    }
}
