using MoneyCheck.Interfaces;
using MoneyCheck.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoneyCheck.Helpers
{
    public class RequestHelper : IRequestMethods
    {
        private HttpClient client = new HttpClient();

        public async Task<TResult> GetRequestAsync<TResult>(string path, string token)
        {
            string url = $"{App.BaseUrl}/{path}";
            var message = new HttpRequestMessage(HttpMethod.Get, url);
            message.Headers.Add("Cookie", $"cmAuthToken={token}");
            var responseMessage = await client.SendAsync(message);

            ResponseModel responseModel = new ResponseModel()
            {
                statusCode = responseMessage.StatusCode,
                result = await responseMessage.Content.ReadAsStringAsync() ?? default
            };
            TResult result = JsonSerializer.Deserialize<TResult>(JsonSerializer.Serialize(responseModel));

            return result;
        }
        public async Task<TResult> PostRequestAsync<TResult>(string path, object value, string token = default)
        {
            string url = $"{App.BaseUrl}/{path}";
            var json = JsonSerializer.Serialize(value);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            if(token != default)
                data.Headers.Add("Cookie", $"cmAuthToken={token}");

            var responseMessage = await client.PostAsync(url, data);

            ResponseModel response = new ResponseModel()
            {
                statusCode = responseMessage.StatusCode,
                result = await responseMessage.Content.ReadAsStringAsync() ?? default
            };
            TResult result = JsonSerializer.Deserialize<TResult>(JsonSerializer.Serialize(response));

            return result;
        }
    }
}
