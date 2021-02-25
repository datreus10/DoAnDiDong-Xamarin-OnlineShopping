using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoAnDiDong.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SPTheoLoai : ContentPage
    {
        public SPTheoLoai()
        {
            InitializeComponent();
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}