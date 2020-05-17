using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace OurSpotKCImporter.WordPressImporter
{
    public class ResourceDataModel
    {
        public AcfDataModel Acf { get; set; }
        public TitleDataModel Title { get; set; }
        public ContentDataModel Content { get; set; }
    }
    
    public class TitleDataModel
    {
        public string Rendered { get; set; }
    }

    public class AcfDataModel
    {
        [JsonProperty(PropertyName = "resource_link")]
        public string ResourceLink { get; set; }
        public string Category { get; set; }
        public string Address { get; set; }
        [JsonProperty(PropertyName = "phone_number")]
        public string PhoneNumber { get; set; }
        [JsonProperty(PropertyName = "resource_icon_image")]
        public string ResourceIconImage { get; set; }
    }

    public class ContentDataModel
    {
        public string Rendered { get; set; }
        public bool Protected { get; set; }
    }
}
