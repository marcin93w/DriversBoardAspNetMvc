using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Driver.WebSite.Source.Security
{
    public class AppSensorClient
    {
        private const string AppSensorUrl = "http://localhost:8085/api/v1.0/events";

        private readonly HttpClient client;
        private readonly JsonSerializerSettings JsonSerializerSettings;

        public AppSensorClient()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Appsensor-Client-Application-Name2", "myclientapp");
            JsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public async Task ReportEventAsync(Event @event)
        {
            var json = JsonConvert.SerializeObject(@event, JsonSerializerSettings);

            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(AppSensorUrl, stringContent);

            var responseString = await response.Content.ReadAsStringAsync();
        }
    }
}