using DoAnDiDong.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace DoAnDiDong.ViewModel
{
    public class CTHDViewModel :BaseViewModel
    {
        public ObservableCollection<SanPham> LstSP { get; set; }

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

        public CTHDViewModel()
        {
            LstSP = new ObservableCollection<SanPham>();
            MessagingCenter.Subscribe<HoaDonViewModel, int>(this, "hd", async (sender,arg)=>
            {
                HttpClient http = new HttpClient();
                var temp = await http.GetStringAsync("http://datreus123.somee.com/api/serviceController/LayCTHD?mahd=" + arg.ToString());
                var lstSP = JsonConvert.DeserializeObject<List<SanPham>>(temp);
                foreach (SanPham item in lstSP)
                    LstSP.Add(item);
                TinhTongTien();
            });
        }

        private void TinhTongTien()
        {
            long temp = 0;
            foreach (SanPham sp in LstSP)
            {
                temp += sp.DonGia * sp.SL;
            }
            if (temp == 0)
                TongTien = "0đ";
            else
                TongTien = String.Format("{0:0,##0} đ", temp);
        }

    }
}
