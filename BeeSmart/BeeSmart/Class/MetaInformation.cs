using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CodeShare.Library.Models.MetaData
{

    public class MetaInformation
    {
        public bool HasData { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string ImageUrl { get; set; }
        public string SiteName { get; set; }
        public ImageButton imgLink { get; set; }
        public Frame Frame { get; set; }
        public MetaInformation(string url)
        {
            Url = url;
            HasData = false;
        }

        public MetaInformation(string url, string title, string description, string keywords, string imageUrl, string siteName)
        {
            Url = url;
            Title = title;
            Description = description;
            Keywords = keywords;
            ImageUrl = imageUrl;
            SiteName = siteName;
        }
        protected MetaInformation(MetaInformation clone)
        {
            this.Url = clone.Url;
            this.Title = clone.Title;
            this.ImageUrl = clone.ImageUrl;
            this.Description = clone.Description;
            this.SiteName = clone.SiteName;
            this.Keywords = clone.Keywords;
         
        }

        public MetaInformation Clone()
        {
            return new MetaInformation(this);
        }
    }
}
