using DoAnDiDong.Model;
using DoAnDiDong.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DoAnDiDong.ViewModel
{
    public class DanhMucViewModel : BaseViewModel
    {

        List<LoaiSanPham> dm;
        public List<LoaiSanPham> DanhMuc
        {
            get { return dm; }
            set { dm = value; RaisePropertyChanged("DanhMuc"); }
        }

        public DanhMucViewModel()
        {
            KhoiTao();
        }
        private async void KhoiTao()
        {
            HttpClient http = new HttpClient();
            var temp = await http.GetStringAsync("http://datreus123.somee.com/api/serviceController/GetAllLoaiSP");
            DanhMuc = JsonConvert.DeserializeObject<List<LoaiSanPham>>(temp);
        }

        LoaiSanPham itemSelected;
        public LoaiSanPham ItemSelected
        {
            get { return itemSelected; }
            set
            {
                itemSelected = value;

                if (itemSelected != null)
                {
                    Page newPage = new SPTheoLoai();
                    newPage.Title = $"{ItemSelected.TenLSP}";
                    Shell.Current.Navigation.PushAsync(newPage);
                    MessagingCenter.Send<DanhMucViewModel, int>(this, "malsp", itemSelected.MaLSP);
                    //LaySPTheoLoai();
                    ItemSelected = null;
                    RaisePropertyChanged("ItemSelected");
                }
            }
        }

        

    }
}
