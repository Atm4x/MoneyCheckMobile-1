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
    public static class Requests
    {
        static RequestHelper _requestHelper = new RequestHelper();
        public static async Task<ResponseModel> GetPurchasesAsync()
        {
            string url = $"api/transactions/get-purchases?filter=none";
            return await _requestHelper.GetRequestAsync<ResponseModel>(url, App.Data.Token);
        }

        public static async Task<ResponseModel> GetBalanceAsync()
        {
            string url = $"api/web/user-balance-stats";
            return await _requestHelper.GetRequestAsync<ResponseModel>(url, App.Data.Token);
        }


        public static async Task<ResponseModel> AddPurchaseAsync(Purchase purchase)
        {
            string url = $"api/transactions/add-purchase";
            return await _requestHelper.PostRequestAsync<ResponseModel>(url, purchase, App.Data.Token);
        }

        public static async Task<ResponseModel> LogInAsync(LogInModel purchase)
        {
            string url = $"auth/api/login";
            return await _requestHelper.PostRequestAsync<ResponseModel>(url, purchase);
        }

        public static async Task<ResponseModel> GetCategoriesAsync()
        {
            string url = $"api/web/get-categories";
            return await _requestHelper.GetRequestAsync<ResponseModel>(url, App.Data.Token);

        }

        public static async Task<ResponseModel> GetStatusAsync()
        {
            string url = $"api/token-ensurer/ensure";
            return await _requestHelper.GetRequestAsync<ResponseModel>(url, App.Data.Token);

        }
    }
}
