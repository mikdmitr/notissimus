using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Net.Http;
using System.Net;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace App2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page2 : ContentPage
    {
        public string[] idArr { get; set; }
        public string[] jsonOffers { get; set; }

        public List<OfferSchema> lOffers;

        public Page2()
        {
            InitializeComponent();
        }

        private async void  button2_Clicked(object sender, EventArgs e)
        {

            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {

                XmlDocument document = new XmlDocument();

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage();
                request.RequestUri = new Uri("https://yastatic.net/market-export/_/partner/help/YML.xml");
                request.Method = HttpMethod.Get;
                request.Headers.Add("Accept", "text/plain");

                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    HttpContent responseContent = response.Content;
                    var stream = await responseContent.ReadAsStreamAsync();

                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.DtdProcessing = DtdProcessing.Ignore;
                    settings.MaxCharactersFromEntities = 1024;

                    using (XmlReader reader = XmlReader.Create(stream, settings))
                    {
                        document.Load(reader);
                    }

                    XmlElement xRoot = document.DocumentElement;

                    XmlNodeList xmlNodeList = xRoot.SelectNodes("shop")[0].SelectNodes("offers")[0].SelectNodes("offer");


                    if (xmlNodeList.Count > 0)
                    {
                        int i = 0;
                        idArr = new string[xmlNodeList.Count];
                        jsonOffers = new string[xmlNodeList.Count];
                        lOffers= new List<OfferSchema>();
                        foreach (XmlNode n in xmlNodeList)
                        {
                            idArr[i] = n.SelectSingleNode("@id").Value;

                            XmlSerializer serializer = new XmlSerializer(typeof(OfferSchema));


                            OfferSchema currentOffer; 

                            using (TextReader reader = new StringReader(n.OuterXml))
                            {
                                currentOffer = (OfferSchema)serializer.Deserialize(reader);
                            }

                            currentOffer.id= n.SelectSingleNode("@id").Value;
                            currentOffer.type = n.SelectSingleNode("@type").Value;

                            lOffers.Add(currentOffer);
                            
                            string json = JsonSerializer.Serialize(currentOffer);
                            jsonOffers[i] = json;

                            i++;
                        }

                        idList.ItemsSource = idArr;
                        await DisplayAlert("Уведомление", "Загружено "+ xmlNodeList.Count.ToString()+" ID-шников", "ОK");
                    }
                    else
                        await DisplayAlert("Уведомление", "ID-шники отсутствуют", "ОK");

                }

            }
            else
            {
                await DisplayAlert("Уведомление", "Соединение c интернет отсутствует", "ОK");
            }

        }

        //private async void button3_Clicked(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new PageOffer());
        //}

        private async void idList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            string id="";
            if (e.Item != null)
                id = e.Item.ToString();

            // await DisplayAlert("Уведомление", id, "ОK");

            
            string serilizedOffer = jsonOffers[Array.IndexOf(idArr, id)];

            await Navigation.PushAsync(new PageOffer(serilizedOffer));
        }
    }
}