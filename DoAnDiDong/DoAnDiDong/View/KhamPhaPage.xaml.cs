using DoAnDiDong.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace DoAnDiDong.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KhamPhaPage : ContentPage
    {
        public int maxRadiusZone = 5000; // m
        public int minRadiusZone = 1000; // m

        //api key lum tren mang (tai khoan phai active biiling acount moi dung dc), 
        //tu khoa google: Share Google Map API key, thu cac key sau neu key chinh ko dung dc
        //AIzaSyDNI_ZWPqvdS6r6gPVO50I4TlYkfkZdXh8
        //AIzaSyDaOulQACiJzBfqumbsqg_-vKha8fCnL-s
        //AIzaSyDc7PnOq3Hxzq6dxeUVaY8WGLHIePl0swY
        //AIzaSyDWTx7bREpM5B6JKdbzOvMW-RRlhkukmVE
        //AIzaSyCJqpC7oo-YYJJ1pRVZJgf84qExlHZCWSc
        //AIzaSyA66KwUrjxcFG5u0exynlJ45CrbrNe3hEc
        //AIzaSyBS6lGj7CsMDE5O9bMEf3I3anmfn34OBlA
        public static string ApiKey = "AIzaSyCJqpC7oo-YYJJ1pRVZJgf84qExlHZCWSc";

        private IList<Pin> lstPin= new List<Pin>();
        private List<double> lstRating=new List<double>();
        public KhamPhaPage()
        {
            InitializeComponent();
            getCurrentLocation();
            lstPin = new List<Pin>();
           
        }
        public async void getCurrentLocation()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    Position p = new Position(location.Latitude, location.Longitude);

                    

                    //di chuyen map ve vi tri cua minh
                    MapSpan mapSpan = MapSpan.FromCenterAndRadius(p, Distance.FromKilometers(1.2));
                    map.MoveToRegion(mapSpan);

                    //to mau zone
                    zonePosition.Center = p;
                    zonePosition.Radius = new Distance(minRadiusZone);
                    zonePosition.StrokeColor = Color.FromHex("#88FF0000");
                    zonePosition.StrokeWidth = 4;
                    zonePosition.FillColor = Color.FromHex("#40FFC0CB");

                    GetDataInZone(p);
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        
        private async void GetDataInZone(Position myPosition)
        {
            // vi tri hien tai cua minh
            var latitude = myPosition.Latitude.ToString("G", CultureInfo.InvariantCulture);
            var longitude = myPosition.Longitude.ToString("G", CultureInfo.InvariantCulture);

            var radius = minRadiusZone.ToString(); // ban kinh tinh bang met
            var type = "electronics_store"; // xem type tai https://developers.google.com/places/supported_types

            HttpClient http = new HttpClient();
            var myJsonResponse = await http.GetStringAsync($"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={latitude},{longitude}&radius={radius}&type={type}&sensor=true&key={ApiKey}");

            
            //cach parse du lieu tu json sang doi tuong : https://json2csharp.com/
            Locations myDeserializedClass = JsonConvert.DeserializeObject<Locations>(myJsonResponse);
     
            while (myDeserializedClass.next_page_token!=null)
            {
                await Task.Delay(1500); // Theo google map api doc: next page token se hieu luc sau mot khoang thoi gian nen phai doi (There is a short delay between when a next_page_token is issued, and when it will become valid.)
                var myJsonResponse_2 = await http.GetStringAsync($"https://maps.googleapis.com/maps/api/place/nearbysearch/json?key={ApiKey}&pagetoken={myDeserializedClass.next_page_token}");  
                Locations temp= JsonConvert.DeserializeObject<Locations>(myJsonResponse_2);
                temp.results.ForEach(item => myDeserializedClass.results.Add(item));
                myDeserializedClass.next_page_token = temp.next_page_token;      
                
            }

            foreach (LocationInfo result in myDeserializedClass.results)
            {
                Pin pin = new Pin
                {
                    Label = result.name,
                    Address = result.vicinity,
                    Type = PinType.Place,
                    Position = new Position(result.geometry.location.lat, result.geometry.location.lng)
                };
                pin.InfoWindowClicked += async (s, args) =>
                {
                    //string pinName = ((Pin)s).Label;
                    await Navigation.PushAsync(new KhamPhaDetailPage(result));                   
                };
                lstRating.Add(result.rating);
                lstPin.Add(pin);
                map.Pins.Add(pin);
            }
            
        }

        

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            map.Pins.Clear();
            for (int i = 0; i < lstRating.Count; i++)
                if (selectedIndex <= lstRating[i])
                    map.Pins.Add(lstPin[i]);
                        
        }

        //private void SliderZone_ValueChanged(object sender, ValueChangedEventArgs e)
        //{
        //    var StepValue = 1;
        //    var newStep = Math.Round(e.NewValue / StepValue);
        //    SliderZone.Value = newStep * StepValue;

        //    var pDistance = new Distance(SliderZone.Value*10+minRadiusZone);
        //    zonePosition.Radius = pDistance;
        //}


    }
}
