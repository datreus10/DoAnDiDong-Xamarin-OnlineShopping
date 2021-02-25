using DoAnDiDong.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;

namespace DoAnDiDong.Data
{
    public static class DataSP
    {
        static public List<SanPham> LSTSP { get; private set; }
        static DataSP()
        {
            LSTSP = new List<SanPham>();
            LayTatCaSP();
        }
        
        private static async void LayTatCaSP()
        {
            HttpClient http = new HttpClient();
            var temp = await http.GetStringAsync("http://www.datreus1234.somee.com/api/serviceController/LayAllSP");
            LSTSP = JsonConvert.DeserializeObject<List<SanPham>>(temp);
        } 
        //private static KhachHang testkh { get; set; }
        //public static KhachHang testKH
        //{
        //    get { return testkh; }
        //    set
        //    {
        //        testkh = value;
        //        NotifyStaticPropertyChanged();
        //    }
        //}

        //public static event PropertyChangedEventHandler StaticPropertyChanged;

        //private static void NotifyStaticPropertyChanged(
        //    [CallerMemberName] string propertyName = null)
        //{
        //    StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        //}

    }
}
