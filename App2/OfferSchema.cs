using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace App2
{
    [XmlRoot("offer")]
    public class OfferSchema
    {
        public string id { get; set; }
        public string type { get; set; }
        [XmlElement("url")]
        public string url { get; set; }
        [XmlElement("price")]
        public int price { get; set; }
        [XmlElement("currencyId")]
        public string currencyId { get; set; }
        [XmlElement("picture")]
        public string picture { get; set; }


    }

    

}
