using DoAnDiDong.Model;
using DoAnDiDong.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DoAnDiDong.ViewModel
{
    public class SPTheoLoaiViewModel : BaseViewModel
    {
        public ICommand SortAscCommand { get; }
        public ICommand SortDescCommand { get; }
        List<SanPham> lstsp;
        public List<SanPham> LstSP
        {
            get { return lstsp; }
            set
            {
                lstsp = value;
                RaisePropertyChanged("LstSP");
            }
        }

        public SPTheoLoaiViewModel()
        {
            MessagingCenter.Subscribe<DanhMucViewModel, int>(this, "malsp", async (sender, arg) =>
            {
                HttpClient http = new HttpClient();
                var temp = await http.GetStringAsync("http://datreus123.somee.com/api/serviceController/GetSPTheoLoai?malsp=" + arg.ToString());
                LstSP = JsonConvert.DeserializeObject<List<SanPham>>(temp);
            });
            SortAscCommand = new Command(() =>
            {
                var temp= LstSP.OrderBy(o =>o.DonGia).ToList();
                LstSP = temp;
            });
            SortDescCommand = new Command(() =>
            {
                var temp = LstSP.OrderByDescending(o => o.DonGia).ToList();
                LstSP = temp;
            });
        }

        SanPham itemSelected;
        public SanPham ItemSelected
        {
            get { return itemSelected; }
            set
            {
                itemSelected = value;

                if (itemSelected != null)
                {
                    Shell.Current.Navigation.PushAsync(new ChiTietSanPham());
                    //MessagingCenter.Send<SPTheoLoaiViewModel, int>(this, "masp", itemSelected.MaSP);
                    MessagingCenter.Send<SPTheoLoaiViewModel, SanPham>(this, "masp", itemSelected);
                    ItemSelected = null;
                    RaisePropertyChanged("ItemSelected");
                }
            }
        }
    }
}
