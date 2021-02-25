using DoAnDiDong.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoAnDiDong.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KhamPhaDetailPage : ContentPage
    {
        public KhamPhaDetailPage()
        {
            InitializeComponent();
        }
        public KhamPhaDetailPage(LocationInfo l)
        {
            InitializeComponent();
            _Location = l;
            BindingContext = this;
            KhoiTao(l.place_id);
            
        }

        private async void KhoiTao(string place_id)
        {

            var api_key = KhamPhaPage.ApiKey;
            HttpClient http = new HttpClient();
            var myJsonResponse = await http.GetStringAsync($"https://maps.googleapis.com/maps/api/place/details/json?place_id={place_id}&fields=photos,reviews&key={api_key}&language=vi");
            LocationReview myDeserializedClass = JsonConvert.DeserializeObject<LocationReview>(myJsonResponse);
            if (myDeserializedClass.result.reviews != null)
                foreach (Review item in myDeserializedClass.result.reviews)
                    _reviews.Add(item);
            else
            {
                ReviewView.IsVisible = false;
                txtComment.IsVisible = true;
            }
                
            if (myDeserializedClass.result.photos != null)
            {
                foreach (Photo item in myDeserializedClass.result.photos)
                    _images.Add($"https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference={item.photo_reference}&key={api_key}");

                Device.StartTimer(TimeSpan.FromSeconds(3), (Func<bool>)(() =>
                {
                    CarouselViewImage.Position = (CarouselViewImage.Position + 1) % _Images.Count();
                    return true;
                }));
            }
            else
            {
                _images.Add("default_geocode.png");
                CarouselViewImage.HeightRequest = 200;
            }
                
        }
        ObservableCollection<Review> _reviews = new ObservableCollection<Review>();
        public ObservableCollection<Review> _Reviews { get { return _reviews; } }

        ObservableCollection<string> _images = new ObservableCollection<string>();
        public ObservableCollection<string> _Images { get { return _images; } }

        public LocationInfo _Location { get; set; }
    }
}