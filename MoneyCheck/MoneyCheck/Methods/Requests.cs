using MoneyCheck.Helpers;
using MoneyCheck.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoneyCheck.Methods
{
    public class Requests
    {
        public static ResponseModel GetPurchasesResponse()
        {
            string url = $"api/transactions/get-purchases?filter=none";
            var response = RequestHelper.GetRequestAsync(url, App.Data.Token).Result;
            return response;
            //var message = new HttpRequestMessage(HttpMethod.Get, url);

            //var client = new HttpClient();

            //var c = client.SendAsync(message);

            
            //return new ResponseModel()
            //{
            //    statusCode = c.Result.StatusCode,
            //    result = c.Result.Content.ReadAsStringAsync().Result ?? default
            //};
        }

        public static ResponseModel GetBalanceResponse()
        {
            string url = $"api/web/user-balance-stats";
            var response = RequestHelper.GetRequestAsync(url, App.Data.Token).Result;
            return response;
            //var message = new HttpRequestMessage(HttpMethod.Get, url);

            //var client = new HttpClient();

            //var c = client.SendAsync(message);


            //return new ResponseModel()
            //{
            //    statusCode = c.Result.StatusCode,
            //    result = c.Result.Content.ReadAsStringAsync().Result ?? default
            //};
        }


        public static ResponseModel AddPurchase(Purchase purchase)
        {
            string url = $"api/transactions/add-purchase";
            var response = RequestHelper.PostRequestAsync(url, App.Data.Token, purchase).Result;
            return response;
            //var json = JsonSerializer.Serialize(purchase);
            //var data = new StringContent(json, Encoding.UTF8, "application/json");

            //var client = new HttpClient();

            //var c = client.PostAsync(url, data);


            //return new ResponseModel()
            //{
            //    statusCode = c.Result.StatusCode,
            //    result = c.Result.Content.ReadAsStringAsync().Result ?? default
            //};
        }


        public static ResponseModel GetCategories()
        {
            string url = $"api/web/get-categories";
            var response = RequestHelper.GetRequestAsync(url, App.Data.Token).Result;
            return response;

            //var message = new HttpRequestMessage(HttpMethod.Get, url);

            //var client = new HttpClient();

            //var c = client.SendAsync(message);


            //return new ResponseModel()
            //{
            //    statusCode = c.Result.StatusCode,
            //    result = c.Result.Content.ReadAsStringAsync().Result ?? default
            //};
        }

        //public static Category GetCategory(long id)
        //{
        //    string url = $"{App.BaseUrl}/api/web/get-categories?token={App.Data.Token}";

        //    var message = new HttpRequestMessage(HttpMethod.Get, url);

        //    var client = new HttpClient();

        //    var c = client.SendAsync(message);

        //    ResponseModel response = new ResponseModel()
        //    {
        //        statusCode = c.Result.StatusCode,
        //        result = c.Result.Content.ReadAsStringAsync().Result ?? default
        //    };

        //    return response.GetParsed<List<Category>>()?.Find(x => x.Id == id) ?? null;
        //}

        public static ResponseModel GetStatus()
        {
            string url = $"api/token-ensurer/ensure";
            var response = RequestHelper.GetRequestAsync(url, App.Data.Token).Result;
            return response;

            //var message = new HttpRequestMessage(HttpMethod.Get, url);

            //var client = new HttpClient();

            //var c = client.SendAsync(message);

            //return new ResponseModel()
            //{
            //    statusCode = c.Result.StatusCode,
            //    result = c.Result.Content.ReadAsStringAsync().Result ?? default
            //};
        }
    }
}
