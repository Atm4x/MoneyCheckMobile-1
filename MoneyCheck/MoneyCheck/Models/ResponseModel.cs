using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MoneyCheck.Models
{
    public partial class ResponseModel
    {
        public HttpStatusCode statusCode { get; set; }
        public string result { get; set; }

        public static bool TryParse<T>(ResponseModel response, out T result)
        {
            if (response?.statusCode == HttpStatusCode.OK)
            {
                result = JsonSerializer.Deserialize<T>(response.result, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
                return result != null;
            }
            result = default;
            return false;
        }

        public static T Parse<T>(ResponseModel response)
        {
            if (response?.statusCode == HttpStatusCode.OK)
            {
                var result = JsonSerializer.Deserialize<T>(response.result, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
                return result;
            }
            return default;
        }

        public T GetParsed<T>()
        {
            if (this?.statusCode == HttpStatusCode.OK)
            {
                var result = JsonSerializer.Deserialize<T>(this?.result, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
                return result;
            }
            return default;
        }
    }
}
