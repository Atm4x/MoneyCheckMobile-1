using MoneyCheck.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoneyCheck.Methods
{
    public class Methods
    {
        public  static ResponseModel GetPurchasesResponse()
        {
            string url = $"{App.baseUrl}/api/transactions/get-purchases?filter=none&token={App.Data.Token}";

            var message = new HttpRequestMessage(HttpMethod.Get, url);

            var client = new HttpClient();

            var c = client.SendAsync(message);

            
            return new ResponseModel()
            {
                statusCode = c.Result.StatusCode,
                result = c.Result.Content.ReadAsStringAsync().Result ?? default
            };
        }

        public static ResponseModel GetBalanceResponse()
        {
            string url = $"{App.baseUrl}/api/web/user-balance-stats?token={App.Data.Token}";

            var message = new HttpRequestMessage(HttpMethod.Get, url);

            var client = new HttpClient();

            var c = client.SendAsync(message);


            return new ResponseModel()
            {
                statusCode = c.Result.StatusCode,
                result = c.Result.Content.ReadAsStringAsync().Result ?? default
            };
        }


        public static ResponseModel AddPurchase(Purchase purchase)
        {
            string url = $"{App.baseUrl}/api/transactions/add-purchase?token={App.Data.Token}";

            var json = JsonSerializer.Serialize(purchase);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var client = new HttpClient();

            var c = client.PostAsync(url, data);


            return new ResponseModel()
            {
                statusCode = c.Result.StatusCode,
                result = c.Result.Content.ReadAsStringAsync().Result ?? default
            };
        }


        public static ResponseModel GetCategories()
        {
            string url = $"{App.baseUrl}/api/web/get-categories?token={App.Data.Token}";

            var message = new HttpRequestMessage(HttpMethod.Get, url);

            var client = new HttpClient();

            var c = client.SendAsync(message);


            return new ResponseModel()
            {
                statusCode = c.Result.StatusCode,
                result = c.Result.Content.ReadAsStringAsync().Result ?? default
            };
        }

        public static Category GetCategory(long id)
        {
            string url = $"{App.baseUrl}/api/web/get-categories?token={App.Data.Token}";

            var message = new HttpRequestMessage(HttpMethod.Get, url);

            var client = new HttpClient();

            var c = client.SendAsync(message);

            ResponseModel response = new ResponseModel()
            {
                statusCode = c.Result.StatusCode,
                result = c.Result.Content.ReadAsStringAsync().Result ?? default
            };

            return response.GetParsed<List<Category>>()?.Find(x => x.Id == id) ?? null;
        }
    }
}
