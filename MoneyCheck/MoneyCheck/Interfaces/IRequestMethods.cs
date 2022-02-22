using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoneyCheck.Interfaces
{
    public interface IRequestMethods
    {
        Task<TResult> GetRequestAsync<TResult>(string path, string token);
        Task<TResult> PostRequestAsync<TResult>(string path, object value, string token = default);
    }
}
