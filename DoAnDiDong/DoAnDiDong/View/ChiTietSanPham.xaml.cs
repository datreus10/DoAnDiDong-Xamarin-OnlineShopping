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
    public partial class ChiTietSanPham : ContentPage
    {
        public ChiTietSanPham()
        {
            InitializeComponent();
            Shell.SetTabBarIsVisible(this, false); //an tab bar o duoi di
        }
    }
}