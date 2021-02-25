using DoAnDiDong.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DoAnDiDong.ViewModel
{

    public class GioHangViewModel : BaseViewModel
    {

        public ObservableCollection<SanPham> LstSP { get; set; }
        public List<SanPham> LstDeleteSP { get; set; }

        private bool showDelButton = false;
        public bool ShowDelButton
        {
            get { return showDelButton; }
            set
            {
                showDelButton = value;
                RaisePropertyChanged("ShowDelButton");
            }
        }
        public ICommand DeleteSPCommand { get; set; }
        private ICommand checkboxCommand;
        public ICommand CheckboxCommand
        {
            get
            {
                if (checkboxCommand == null)
                {
                    checkboxCommand = new Command(e =>
                    {
                        //var result = await Shell.Current.DisplayAlert("Thông báo", "Bạn muốn xóa sản phẩm khỏi giỏ hàng", "Không", "Có");
                        //if (!result)
                        //{
                        //    var item = (e as SanPham);
                        //    LstSP.Remove(item);
                        //    DeleteSP(item.MaSP.ToString());
                        //    TinhTongTien();
                        //}
                        //if (LstSP.Count == 0)
                        //    EmptyCart = false;
                        var item = (e as SanPham);
                        if (!LstDeleteSP.Contains(item))
                            LstDeleteSP.Add(item);
                        else
                            LstDeleteSP.Remove(item);
                    });
                }
                return checkboxCommand;
            }
        }

        private ICommand tangSLCommand;
        public ICommand TangSLCommand
        {
            get
            {

                if (tangSLCommand == null)
                {
                    tangSLCommand = new Command((e) =>
                    {
                        var item = (e as SanPham);
                        if (item.SL < 10)
                        {
                            item.SL += 1;
                            LstSP[LstSP.IndexOf(item)] = item;
                            TinhTongTien();
                            UpdateSLSP(item.MaSP, 1);
                        }
                    });
                }
                return tangSLCommand;
            }


        }

        private ICommand giamSLCommand;
        public ICommand GiamSLCommand
        {
            get
            {

                if (giamSLCommand == null)
                {
                    giamSLCommand = new Command((e) =>
                    {
                        var item = (e as SanPham);
                        if (item.SL > 1)
                        {
                            item.SL -= 1;
                            LstSP[LstSP.IndexOf(item)] = item;
                            TinhTongTien();
                            UpdateSLSP(item.MaSP, -1);
                        }
                    });
                }
                return giamSLCommand;
            }
        }

        string tongTien;
        public string TongTien
        {
            get { return tongTien; }
            set
            {
                tongTien = value;
                RaisePropertyChanged("TongTien");
            }
        }

        private bool emptyCart = false;
        public bool EmptyCart
        {
            get { return emptyCart; }
            set
            {
                emptyCart = value;
                ShowDelButton = emptyCart;
                RaisePropertyChanged("EmptyCart");
            }
        }


        public ICommand DatHang { get; private set; }

        public GioHangViewModel()
        {
            LstSP = new ObservableCollection<SanPham>();
            LstDeleteSP = new List<SanPham>();
            if (userID != -1)
                LaySanPhamGioHang();
            MessagingCenter.Subscribe<LoginViewModel, KhachHang>(this, "logined", (sender, arg) =>
            {
                LaySanPhamGioHang();
            });
            MessagingCenter.Subscribe<TaiKhoanViewModel>(this, "logout", sender =>
            {
                LstSP.Clear();
                TongTien = "Tổng tiền: 0đ";
                EmptyCart = false;
            });
            MessagingCenter.Subscribe<ChiTietSanPhamViewModel, SanPham>(this, "logined", (sender, arg) =>
            {
                arg.SL = 1;
                LstSP.Add(arg);
                TinhTongTien();
                if (LstSP.Count == 0)
                    EmptyCart = false;
                else
                    EmptyCart = true;
            });
            DatHang = new Command(datHang);
            DeleteSPCommand = new Command(async () =>
            {
                if (LstDeleteSP.Count != 0)
                {
                    var result = await Shell.Current.DisplayAlert("Thông báo", "Bạn muốn xóa sản phẩm khỏi giỏ hàng", "Không", "Có");
                    if (!result)
                    {
                        LstDeleteSP.ForEach(item => {
                            DeleteSP(item.MaSP.ToString());
                            LstSP.Remove(item);
                        });
                        LstDeleteSP.Clear();
                        TinhTongTien();
                        if (LstSP.Count == 0)
                            EmptyCart = false;
                    }
                }

            });
        }


        private async void DeleteSP(string masp)
        {
            HttpClient http = new HttpClient();
            await http.GetStringAsync($"http://datreus123.somee.com/api/serviceController/XoaSPGioHang?makh={userID}&masp={masp}");
        }

        private async void LaySanPhamGioHang()
        {
            HttpClient http = new HttpClient();
            var temp = await http.GetStringAsync("http://datreus123.somee.com/api/serviceController/GetGioHangTheoMaKH?makh=" + userID.ToString());
            var lstSP = JsonConvert.DeserializeObject<List<SanPham>>(temp);
            foreach (SanPham item in lstSP)
                LstSP.Add(item);
            TinhTongTien();
            if (LstSP.Count == 0)
                EmptyCart = false;
            else
                EmptyCart = true;
        }
        private void TinhTongTien()
        {
            long temp = 0;
            foreach (SanPham sp in LstSP)
            {
                temp += sp.DonGia * sp.SL;
            }
            if (temp == 0)
                TongTien = "Tổng tiền: 0đ";
            else
                TongTien = String.Format("Tổng tiền: {0:0,##0} đ", temp);
        }

        private async void UpdateSLSP(int masp, int n)
        {
            HttpClient http = new HttpClient();
            await http.GetStringAsync($"http://datreus123.somee.com/api/serviceController/ThayDoiSLGioHang?makh={userID}&masp={masp}&val={n}");
        }

        private async void datHang()
        {

            var result = await Shell.Current.DisplayAlert("Thông báo", "Bạn muốn đặt hàng", "Không", "Có");
            if (!result)
            {
                HttpClient http = new HttpClient();
                var ngayhd = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                string mahd = await http.GetStringAsync($"http://datreus123.somee.com/api/serviceController/TaoHD?makh={userID}&ngayhd={ngayhd}");
                foreach (SanPham item in LstSP)
                {
                    await http.GetStringAsync($"http://datreus123.somee.com/api/serviceController/TaoCTHD?mahd={mahd}&masp={item.MaSP}&sl={item.SL}");
                }
                await Shell.Current.DisplayAlert("Thông báo", "Đặt hàng thành công", "OK");
                LstSP.Clear();
                EmptyCart = false;
            }


        }
    }



}
