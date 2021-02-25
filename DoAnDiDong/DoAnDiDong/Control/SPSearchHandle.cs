using DoAnDiDong.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using DoAnDiDong.Data;
using System.Linq;
using DoAnDiDong.Model;
using DoAnDiDong.ViewModel;

namespace DoAnDiDong.Control
{
    public class SPSearchHandle : SearchHandler
    {
        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (string.IsNullOrWhiteSpace(newValue))
            {
                ItemsSource = null;
            }
            else
            {
                ItemsSource = DataSP.LSTSP
                    .Where(item => item.TenSP.ToLower().Contains(newValue.ToLower()))
                    .ToList<SanPham>();
            }
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
            await Task.Delay(1000);
            var viewmodel = new ChiTietSanPhamViewModel { CTSP = (SanPham)item };
            var detailPage = new ChiTietSanPham { BindingContext = viewmodel };
            await Shell.Current.Navigation.PushAsync(detailPage);
        }
    }
}
