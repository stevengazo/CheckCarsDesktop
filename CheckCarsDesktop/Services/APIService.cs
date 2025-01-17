using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CheckCarsDesktop.Services
{
    public class APIService
    {
        public string Token { get; set; }
        private readonly HttpClient _httpClient;

        public APIService(TimeSpan? timeout = null)
        {
           _httpClient = new HttpClient
            {
                BaseAddress = new Uri($"https://mecsacars.stevengazo.co.cr/"),
                Timeout = timeout ?? TimeSpan.FromSeconds(100) // Configuración predeterminada de tiempo de espera
            };
        }

        private void AddAuthorizationHeader()
        {
            if (!string.IsNullOrEmpty(Token))
            {
                // Si el token existe, se agrega al encabezado
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            }
        }
        public async Task<T?> GetAsync<T>(string endpoint, TimeSpan? timeout = null, bool useToken = false)
        {
            if(useToken)
            {
                AddAuthorizationHeader();
            }
           return await GetAsync<T>(endpoint, timeout);
        }
        public async Task<T?> GetAsync<T>(string endpoint, TimeSpan? timeout = null)
        {
            try
            {
                using var cts = timeout.HasValue ? new CancellationTokenSource(timeout.Value) : null;
                var response = await _httpClient.GetAsync(endpoint, cts?.Token ?? CancellationToken.None);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(json);
                }
                return default;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error en GET: {e.Message}");
                return default;
            }
        }

        public async Task<string?> PostAsync<T>(string endpoint, T data, TimeSpan? timeout = null)
        {
            try
            {
                using var cts = timeout.HasValue ? new CancellationTokenSource(timeout.Value) : null;
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(endpoint, content, cts?.Token ?? CancellationToken.None);

                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return responseBody;
                }

                Console.WriteLine($"Error en POST: {response.StatusCode} - {responseBody}");
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error en POST: {e.Message}");
                return null;
            }
        }


    }

}
