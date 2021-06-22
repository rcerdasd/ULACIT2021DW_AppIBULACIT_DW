using AppIBULACIT.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AppIBULACIT.Controllers
{
    public class MonedaManager
    {
        string UrlBase = "http://localhost:49220/api/Moneda/";

        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;
        }

        public async Task<Moneda> GetId(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Moneda>(response);
        }

        public async Task<IEnumerable<Moneda>> GetAll(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Moneda>>(response);
        }

         public async Task<Moneda> Ingresar (Moneda moneda, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase, new StringContent(JsonConvert.SerializeObject(moneda), Encoding.UTF8, "application/jon"));

            return JsonConvert.DeserializeObject<Moneda>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Moneda> Actualizar(Moneda moneda, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase, new StringContent(JsonConvert.SerializeObject(moneda), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Moneda>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Moneda> Eliminar(string token, string id)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, id));

            return JsonConvert.DeserializeObject<Moneda>(await response.Content.ReadAsStringAsync());
        }
    }
}