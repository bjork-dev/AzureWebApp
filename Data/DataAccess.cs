using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using AzureWebApp.Models;

namespace AzureWebApp.Data
{
    public class DataAccess : IDataAccess
    {
        private readonly HttpClient _client = new();
        
        private readonly SecretClientOptions _options = new()
        {
            Retry =
            {
                Delay= TimeSpan.FromSeconds(2),
                MaxDelay = TimeSpan.FromSeconds(16),
                MaxRetries = 5,
                Mode = RetryMode.Exponential
            }
        };

        public async Task<Todo[]> GetAsync()
        {
            string code = await GetSecret("GetTodo");
            Todo[] todo = null;
            HttpResponseMessage response = await _client.GetAsync($"https://todo-project.azurewebsites.net/api/todo?code={code}");

            if (response.IsSuccessStatusCode)
                todo = await response.Content.ReadFromJsonAsync<Todo[]>();
            return todo;
        }

        public async Task<HttpResponseMessage> PostAsync(Todo todo)
        {
            string code = await GetSecret("AddTodo");
            var json = JsonSerializer.Serialize(todo);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync($"https://todo-project.azurewebsites.net/api/todo?code={code}", data);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteAsync(string id)
        {
            string code = await GetSecret("DeleteTodo");
            return await _client.DeleteAsync($"https://todo-project.azurewebsites.net/api/todo/{id}?code={code}");
        }

        private async Task<string> GetSecret(string secretName)
        {
            var client = new SecretClient(new Uri("https://bjorkdev.vault.azure.net/"), new DefaultAzureCredential(), _options);
            KeyVaultSecret secret = await client.GetSecretAsync(secretName);
            return secret.Value;
        }
    }
}
