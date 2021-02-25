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
    class TaiKhoanViewModel: BaseViewModel
    {
       
        public ICommand LoginCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand HoaDonCommand { get; }
        public ICommand TaiKhoanCommand { get; }

        private string xinchao = "Chào mừng quý khách";
        private bool login = true;
        
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
        public string XinChao
        {
            get { return xinchao; }
            set { xinchao = value; RaisePropertyChanged("XinChao"); }
        }
        public bool Login
        {
            get { return login; }
            set { login = value;
                
                RaisePropertyChanged("Login"); }
        }
        private UriImageSource userImg;
        public UriImageSource UserImg
        {
            get { return userImg; }
            set
            {
                userImg = value;
                RaisePropertyChanged("UserImg");
            }
        }

        public TaiKhoanViewModel()
        {
            KH = new KhachHang();
            UserImg = new UriImageSource { CachingEnabled = false, Uri = new Uri(KH.usrImg) };
            LoginCommand = new Command(() =>
            {
                Shell.Current.Navigation.PushAsync(new LoginPage());
            });
            LogoutCommand = new Command(() =>
            {
                KH = new KhachHang();
                XinChao= "Chào mừng quý khách";
                Login = true;
                userID= -1;
                MessagingCenter.Send<TaiKhoanViewModel>(this, "logout");
                Shell.Current.GoToAsync("//main/home");
                UserImg = new UriImageSource { CachingEnabled = false, Uri = new Uri(KH.usrImg) };
            });
            HoaDonCommand = new Command(() =>
            {
                if (!Login)
                    Shell.Current.Navigation.PushAsync(new HoaDonPage());
                else
                    Shell.Current.Navigation.PushAsync(new LoginPage());
            });
            TaiKhoanCommand = new Command(() =>
            {
                if (!Login)
                {
                    Shell.Current.Navigation.PushAsync(new Profile());
                    MessagingCenter.Send<TaiKhoanViewModel, KhachHang>(this, "profile", KH);
                }
                    
                else
                    Shell.Current.Navigation.PushAsync(new LoginPage());
            });
            MessagingCenter.Subscribe<LoginViewModel, KhachHang>(this, "logined", (sender, arg) =>
            {
                if (string.IsNullOrEmpty(arg.usrImg))
                    arg.usrImg = "http://www.datreus1234.somee.com/Media/imgUser.png";
                KH = arg;
                UserImg = new UriImageSource { CachingEnabled = false, Uri = new Uri(KH.usrImg) };
                Login = false;
                XinChao = "Chào mừng, " + KH.HoTen;
                MessagingCenter.Send<TaiKhoanViewModel>(this, "logined");

            });
            MessagingCenter.Subscribe<ProfileViewModel, string>(this, "updateImageUser", (sender, arg) => {
                UserImg = new UriImageSource { CachingEnabled = false, Uri = new Uri(arg) };
            });

        }
        

        
    }
}
