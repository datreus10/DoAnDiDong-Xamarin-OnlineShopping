using DoAnDiDong.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DoAnDiDong.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected BaseViewModel()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }


        //static public KhachHang tempKH = new KhachHang
        //{
        //    MaKH = -1
        //};
        static public int userID = -1;
    }
}
