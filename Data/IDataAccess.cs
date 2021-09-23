using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AzureWebApp.Models;

namespace AzureWebApp.Data
{
    public interface IDataAccess
    {
        Task<Todo[]> GetAsync();
        Task<HttpResponseMessage> PostAsync(Todo todo);
        Task<HttpResponseMessage> DeleteAsync(string id);
    }
}
