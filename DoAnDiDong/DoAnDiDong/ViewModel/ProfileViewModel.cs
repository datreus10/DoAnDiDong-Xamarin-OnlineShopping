using DoAnDiDong.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Net.Http;

namespace DoAnDiDong.ViewModel
{
    public class ProfileViewModel :BaseViewModel
    {
        KhachHang kh;
        string gioiTinh;
        public string GioiTinh
        {
            get { return gioiTinh; }
            set
            {
                gioiTinh = value;
                RaisePropertyChanged("GioiTinh");
            }
        }
        public KhachHang KH
        {
            get { return kh; }
            set
            {
                kh = value;
                RaisePropertyChanged("KH");
            }
        }
        private bool loading;
        public bool Loading
        {
            get { return loading; }
            set { loading = value; RaisePropertyChanged("Loading"); }
        }
        public ICommand updateUserImg { get; set; }
        public ProfileViewModel()
        {
            Loading = false;
            MessagingCenter.Subscribe<TaiKhoanViewModel, KhachHang>(this, "profile", (sender,arg)=>
            {
                KH = arg;
                GioiTinh = KH.GioiTinh ? "Nữ" : "Nam";
            });
            updateUserImg = new Command(async () =>
            {
                var file = await MediaPicker.PickPhotoAsync();

                if (file == null)
                    return;
                Loading = true;
                var fileName = $"KH_{KH.MaKH}" + file.FileName.Substring(file.FileName.LastIndexOf("."));
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(await file.OpenReadAsync()), "file", fileName);

                var httpClient = new HttpClient();
                var response = await httpClient.PostAsync("http://www.datreus1234.somee.com/api/serviceController/PostDocument", content);

                var StatusLabel = response.StatusCode.ToString();

                if (StatusLabel == "OK")
                {

                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    DocumentUploadReponse docReponse = JsonConvert.DeserializeObject<DocumentUploadReponse>(jsonString);
                    var link = docReponse.DocumentUrl[0].Replace("\\", "/");
                    link = "http://" + link.Substring(link.IndexOf("datreus1234") + 12);
                    await httpClient.GetStringAsync($"http://datreus1234.somee.com/api/serviceController/UpdateLinkUserImg?makh={KH.MaKH}&link={link}");
                    await Shell.Current.DisplayAlert("Thông báo", "Cập nhật ảnh thành công", "OK");
                    MessagingCenter.Send<ProfileViewModel, string>(this, "updateImageUser", link);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Thông báo", "Cập nhật ảnh thất bại", "OK");
                }
                Loading = false;
            });
        }
    }
}
