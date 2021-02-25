using System;
using System.Collections.Generic;
using System.Text;

namespace DoAnDiDong.Model
{
    public class Review
    {
        public string author_name { get; set; }
        public string author_url { get; set; }
        public string language { get; set; }
        public string profile_photo_url { get; set; }
        public int rating { get; set; }
        public string relative_time_description { get; set; }
        public string text { get; set; }
        public int time { get; set; }
    }

    public class Reviews
    {
        public List<Photo> photos { get; set; }
        public List<Review> reviews { get; set; }
    }

    public class LocationReview
    {
        public List<object> html_attributions { get; set; }
        public Reviews result { get; set; }
        public string status { get; set; }
    }




}
