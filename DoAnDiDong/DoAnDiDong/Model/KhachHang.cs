using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DoAnDiDong.Model
{
    //public class KhachHang : INotifyPropertyChanged
    //{
    //    public event PropertyChangedEventHandler PropertyChanged;
    //    public void RaisePropertyChanged(string PropertyName)
    //    {
    //        if (PropertyChanged != null)
    //            PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
    //    }
    //    private int maKH;
    //    private string hoTen;
    //    private string diaChi;
    //    private string dienThoai;
    //    private string tenDangNhap;
    //    private string matKhau;
    //    private DateTime ngaySinh;
    //    private bool gioiTinh;
    //    private string email;


    //    public int MaKH
    //    {
    //        get { return maKH; }
    //        set { maKH = value; RaisePropertyChanged("MaKH"); }
    //    }
    //    public string HoTen
    //    {
    //        get { return hoTen; }
    //        set { hoTen = value; RaisePropertyChanged("HoTen"); }
    //    }
    //    public string DiaChi
    //    {
    //        get { return diaChi; }
    //        set { diaChi = value; RaisePropertyChanged("DiaChi"); }
    //    }
    //    public string DienThoai
    //    {
    //        get { return dienThoai; }
    //        set { dienThoai = value; RaisePropertyChanged("DienThoai"); }
    //    }
    //    public string TenDangNhap
    //    {
    //        get { return tenDangNhap; }
    //        set { tenDangNhap = value; RaisePropertyChanged("TenDangNhap"); }
    //    }
    //    public string MatKhau
    //    {
    //        get { return matKhau; }
    //        set { matKhau = value; RaisePropertyChanged("MatKhau"); }
    //    }
    //    public DateTime NgaySinh
    //    {
    //        get { return ngaySinh; }
    //        set { ngaySinh = value; RaisePropertyChanged("NgaySinh"); }
    //    }
    //    public bool GioiTinh
    //    {
    //        get { return gioiTinh; }
    //        set { gioiTinh = value; RaisePropertyChanged("GioiTinh"); }
    //    }
    //    public string Email
    //    {
    //        get { return email; }
    //        set { email = value; RaisePropertyChanged("Email"); }
    //    }

    //}

    public class KhachHang
    {
        public int MaKH { get; set; }
        public string HoTen { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public DateTime NgaySinh { get; set; }
        public bool GioiTinh { get; set; }
        public string Email { get; set; }
        public string usrImg { get; set; } = "http://www.datreus1234.somee.com/Media/imgUser.png";
    }
}
