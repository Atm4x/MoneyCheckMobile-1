using MoneyCheck.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoneyCheck.Helpers
{
    public static class RequestHelper
    {
        private static HttpClient client = new HttpClient();
        public static async Task<ResponseModel> GetRequestAsync(string path, string token)
        {
            string url = $"{App.BaseUrl}/{path}";
            var message = new HttpRequestMessage(HttpMethod.Get, url);
            message.Headers.Add("Cookie", $"cmAuthToken={token}");

            var response = await client.SendAsync(message).ConfigureAwait(false);

            return new ResponseModel()
            {
                statusCode = response.StatusCode,
                result = response.Content.ReadAsStringAsync().Result ?? default
            };
        }
        public static async Task<ResponseModel> PostRequestAsync(string path, string token, object value)
        {
            string url = $"{App.BaseUrl}/{path}";
            var json = JsonSerializer.Serialize(value);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            data.Headers.Add("Cookie", $"cmAuthToken={token}");

            //var message = new HttpRequestMessage(HttpMethod.Post, url);
            //message.Headers.Add("Cookie", $"cmAuthToken={token}");

            var response = await client.PostAsync(json, data).ConfigureAwait(false);

            return new ResponseModel()
            {
                statusCode = response.StatusCode,
                result = response.Content.ReadAsStringAsync().Result ?? default
            };
        }
    }
}
