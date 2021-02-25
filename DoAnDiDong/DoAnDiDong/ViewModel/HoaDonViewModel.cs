using DoAnDiDong.Model;
using DoAnDiDong.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace DoAnDiDong.ViewModel
{
    public class HoaDonViewModel : BaseViewModel
    {
        public ObservableCollection<HoaDon> LstHD { get; set; }
        public ObservableCollection<SanPham> LstSP { get; set; }
        public HoaDonViewModel()
        {
            LstHD= new ObservableCollection<HoaDon>();
            LstSP=new ObservableCollection<SanPham>();
            if (userID!= -1)
            {
                LayHoaDon();
            }
        }
        private async void LayHoaDon()
        {
            HttpClient http = new HttpClient();
            var temp = await http.GetStringAsync("http://datreus123.somee.com/api/serviceController/LayHD?makh=" + userID.ToString());
            var lstHD = JsonConvert.DeserializeObject<List<HoaDon>>(temp);
            lstHD.Sort((x, y) => x.MaHD > y.MaHD ? 1 : x.MaHD<y.MaHD ? -1 : 0);
            foreach (HoaDon item in lstHD)
                LstHD.Add(item);
            
        }

        HoaDon itemSelected;
        public HoaDon ItemSelected
        {
            get { return itemSelected; }
            set
            {
                itemSelected = value;

                if (itemSelected != null)
                {
                    Shell.Current.Navigation.PushAsync(new CTHDPage());
                    MessagingCenter.Send<HoaDonViewModel, int>(this, "hd", itemSelected.MaHD);
                    ItemSelected = null;
                    RaisePropertyChanged("ItemSelected");
                }

            }
        }

       
    }
}
