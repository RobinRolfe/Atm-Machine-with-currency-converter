using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Atm11.Models;

namespace Atm11.Controllers
{
    public class CurrencyClient
    {
        private string base_Url = "http://localhost:55025/api/";

        public IEnumerable<Currency> findAll()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(base_Url);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("currencies").Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseData = response.Content.ReadAsStringAsync().Result;
                    var Currency = JsonConvert.DeserializeObject<List<Currency>>(responseData);
                    return Currency;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
