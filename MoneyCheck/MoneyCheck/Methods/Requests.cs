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
        static RequestHelper _requestHelper = new RequestHelper();
        public static async Task<ResponseModel> GetPurchasesResponse()
        {
            string url = $"api/transactions/get-purchases?filter=none";
            return await _requestHelper.GetRequestAsync<ResponseModel>(url, App.Data.Token);
        }

        public static async Task<ResponseModel> GetBalanceResponse()
        {
            string url = $"api/web/user-balance-stats";
            return await _requestHelper.GetRequestAsync<ResponseModel>(url, App.Data.Token);
        }


        public static async Task<ResponseModel> AddPurchase(Purchase purchase)
        {
            string url = $"api/transactions/add-purchase";
            return await _requestHelper.PostRequestAsync<ResponseModel>(url, purchase, App.Data.Token);
        }

        public static async Task<ResponseModel> LogIn(LogInModel purchase)
        {
            string url = $"auth/api/login";
            return await _requestHelper.PostRequestAsync<ResponseModel>(url, purchase);
        }

        public static async Task<ResponseModel> GetCategories()
        {
            string url = $"api/web/get-categories";
            return await _requestHelper.GetRequestAsync<ResponseModel>(url, App.Data.Token);

        }

        public static async Task<ResponseModel> GetStatus()
        {
            string url = $"api/token-ensurer/ensure";
            return await _requestHelper.GetRequestAsync<ResponseModel>(url, App.Data.Token);

        }
    }
}
