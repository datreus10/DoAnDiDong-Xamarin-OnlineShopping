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
    public class ChiTietSanPhamViewModel : BaseViewModel
    {

        public ICommand MuaCommand { get; private set; }

        SanPham ctsp;
        public SanPham CTSP
        {
            get { return ctsp; }
            set
            {
                ctsp = value;
                RaisePropertyChanged("CTSP");
            }
        }

        public ChiTietSanPhamViewModel()
        {
            MessagingCenter.Subscribe<SPTheoLoaiViewModel, SanPham>(this, "masp", (sender, arg) =>
            {
                CTSP = arg;
            });
            MessagingCenter.Subscribe<MainPageViewModel, SanPham>(this, "masp", (sender, arg) =>
            {
                CTSP = arg;
            });


            MuaCommand = new Command(muaCommand);
        }
        private async void muaCommand()
        {
            if (userID != -1)
            {
                HttpClient http = new HttpClient();
                var temp = await http.GetStringAsync($"http://datreus123.somee.com/api/serviceController/AddGioHang?makh={userID}&masp={CTSP.MaSP}");
                if (temp == "-1")
                    await Shell.Current.DisplayAlert("Thông báo", "Sản phẩm đã có trong giỏ hàng", "OK");
                else
                {
                    await Shell.Current.DisplayAlert("Thông báo", "Đã thêm vào giỏ hàng", "OK");
                    MessagingCenter.Send<ChiTietSanPhamViewModel,SanPham>(this, "logined",CTSP);
                }

            }
            else
            {
                await Shell.Current.DisplayAlert("Thông báo", "Bạn cần đăng nhập để mua", "OK");
                //await Shell.Current.Navigation.PushAsync(new LoginPage());
            }
        }

    }
}
