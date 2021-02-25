using DoAnDiDong.Model;
using DoAnDiDong.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace DoAnDiDong.ViewModel
{
    public class MainPageViewModel:BaseViewModel
    {
        public ObservableCollection<SanPham> LstSP_1 { get; set; }
        public ObservableCollection<SanPham> LstSP_2 { get; set; }
        

        public MainPageViewModel()
        {
            LstSP_1 = new ObservableCollection<SanPham>();
            LstSP_2 = new ObservableCollection<SanPham>();
            LaySP(LstSP_1);
            LaySP(LstSP_2);
        }

        private async void LaySP(ObservableCollection<SanPham> temp_sp)
        {
            HttpClient http = new HttpClient();
            var temp = await http.GetStringAsync("http://datreus123.somee.com/api/serviceController/LayNgauNhien10SP");
            var lstSP= JsonConvert.DeserializeObject<List<SanPham>>(temp);
            foreach (SanPham item in lstSP)
                temp_sp.Add(item);
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
                    MessagingCenter.Send<MainPageViewModel, SanPham>(this, "masp", itemSelected);
                    ItemSelected = null;
                    RaisePropertyChanged("ItemSelected");
                }
            }
        }
    }
}
