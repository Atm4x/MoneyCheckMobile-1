﻿using MoneyCheck.Helpers;
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
            return await _requestHelper.GetRequestAsync<ResponseModel>(url, App.Data?.Token);
        }

        public static async Task<ResponseModel> GetBalanceAsync()
        {
            string url = $"api/web/user-balance-stats";
            return await _requestHelper.GetRequestAsync<ResponseModel>(url, App.Data?.Token);
        }


        public static async Task<ResponseModel> GetInflationForYearAsync()
        {
            string url = $"api/web/get-inflation-for-year";
            return await _requestHelper.GetRequestAsync<ResponseModel>(url, App.Data?.Token);
        }

        public static async Task<ResponseModel> AddPurchaseAsync(Purchase purchase)
        {
            string url = $"api/transactions/add-purchase";
            return await _requestHelper.PostRequestAsync<ResponseModel>(url, purchase, App.Data?.Token);
        }

        public static async Task<ResponseModel> AddDebtorAsync(DebtorType debtor)
        {
            string url = $"api/debtors/add";
            return await _requestHelper.PostRequestAsync<ResponseModel>(url, debtor, App.Data?.Token);
        }
        
        public static async Task<ResponseModel> AddDebtAsync(object debt)
        {
            string url = $"api/debts/add-debt";
            return await _requestHelper.PostRequestAsync<ResponseModel>(url, debt, App.Data?.Token);
        }
        
        public static async Task<ResponseModel> GetDebtorsAsync()
        {
            string url = $"api/web/get-debtors";
            return await _requestHelper.GetRequestAsync<ResponseModel>(url, App.Data?.Token);
        }

        public static async Task<ResponseModel> GetDebtsAsync()
        {
            string url = $"api/debts/get-debts";
            return await _requestHelper.GetRequestAsync<ResponseModel>(url, App.Data?.Token);
        }

        public static async Task<ResponseModel> RemovePurchaseAsync(Purchase purchase)
        {
            string url = $"api/transactions/remove-purchase";
            return await _requestHelper.DeleteRequestAsync<ResponseModel>(url, purchase.Id.Value, App.Data?.Token);
        }

        public static async Task<ResponseModel> EditPurchaseAsync(Purchase purchase)
        {
            string url = $"api/transactions/edit-purchase";
            return await _requestHelper.PatchRequestAsync<ResponseModel>(url, purchase, App.Data?.Token);
        }

        public static async Task<ResponseModel> LogUpAsync(LogUpModel logUp)
        {
            string url = $"auth/api/log-up";
            return await _requestHelper.PostRequestAsync<ResponseModel>(url, logUp);
        }

        public static async Task<ResponseModel> LogInAsync(LogInModel logIn)
        {
            string url = $"auth/api/login";
            return await _requestHelper.PostRequestAsync<ResponseModel>(url, logIn);
        }

        public static async Task<ResponseModel> LogOutAsync()
        {
            string url = $"auth/api/logout?token={App.Data?.Token}";
            return await _requestHelper.PostRequestAsync<ResponseModel>(url, "");
        }

        public static async Task<ResponseModel> GetCategoriesAsync()
        {
            string url = $"api/web/get-categories";
            return await _requestHelper.GetRequestAsync<ResponseModel>(url, App.Data?.Token);

        }

        public static async Task<ResponseModel> GetStatusAsync()
        {
            string url = $"api/token-ensurer/ensure";
            return await _requestHelper.GetRequestAsync<ResponseModel>(url, App.Data?.Token);

        }
    }
}
