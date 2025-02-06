using Anade.Domain.Core;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http.Json;


namespace Anade.Business.Core
{
    public class GenericApiService<T> : IApiService<T> where T : class
    {
        protected readonly ApiServicesUrls _apiServicesUrls;
        protected readonly HttpClient _httpClient;

        public GenericApiService(HttpClient httpClient, ApiServicesUrls apiServicesUrls)
        {
            _httpClient = httpClient;
            _apiServicesUrls = apiServicesUrls;
        }
        public async Task<List<T>> GetAllAsync()
        {
            //TDOD: Generate url for GetAll api service
            string requestUrl = _apiServicesUrls.GetAll<T>();

            var responseString = await _httpClient.GetStringAsync(requestUrl);

            var list = JsonConvert.DeserializeObject<List<T>>(responseString);

            return list;
        }
        public async Task<PagedResult<T>> GetAllPagedAsync(string term, string orderBy, int page = 1, int pageSize = 10)
        {
            //TDOD: Generate url for GetAllPaged api service
            string requestUrl = _apiServicesUrls.GetAllPaged<T>() + $"?term={term}&orderBy={orderBy}&page={page}&pageSize={pageSize}";

            var responseString = await _httpClient.GetStringAsync(requestUrl);

            var list = JsonConvert.DeserializeObject<PagedResult<T>>(responseString);

            return list;
        }
        public async Task<T> GetByIdAsync(int id)
        {
            //TDOD: Generate url for GetById api service
            string requestUrl = _apiServicesUrls.GetByIdUrl<T, int>(id);

            var responseString = await _httpClient.GetStringAsync(requestUrl);

            var entity = JsonConvert.DeserializeObject<T>(responseString);

            return entity;
        }
        public async Task<T> GetByIdAsync(string id)
        {
            //TDOD: Generate url for GetById api service
            string requestUrl = _apiServicesUrls.GetByIdUrl<T, string>(id);

            var responseString = await _httpClient.GetStringAsync(requestUrl);

            var entity = JsonConvert.DeserializeObject<T>(responseString);

            return entity;
        }
        public async Task<BusinessResult> PostAsync(T entity)
        {
            //TDOD: Generate url for Post api service
            BusinessResult actionResult = BusinessResult.Success;

            string requestUrl = _apiServicesUrls.GetBaseUrl<T>();

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUrl, entity);

            response.EnsureSuccessStatusCode();

            actionResult = JsonConvert.DeserializeObject<BusinessResult>(await response.Content.ReadAsStringAsync());

            return actionResult;
        }
        public async Task<BusinessResult> PutAsync(int id, T entity)
        {
            BusinessResult actionResult = BusinessResult.Success;

            //TDOD: Generate url for Put api service
            string requestUrl = _apiServicesUrls.GetByIdUrl<T, int>(id);

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(requestUrl, entity);

            response.EnsureSuccessStatusCode();

            actionResult = JsonConvert.DeserializeObject<BusinessResult>(await response.Content.ReadAsStringAsync());

            return actionResult;
        }
        public async Task<BusinessResult> DeleteAsync(int id, T entity)
        {
            BusinessResult actionResult = BusinessResult.Success;

            //TDOD: Generate url for Delete api service
            string requestUrl = _apiServicesUrls.GetByIdUrl<T, int>(id);

            HttpResponseMessage response = await _httpClient.DeleteAsync(requestUrl);

            response.EnsureSuccessStatusCode();

            actionResult = JsonConvert.DeserializeObject<BusinessResult>(await response.Content.ReadAsStringAsync());

            return actionResult;
        }

    }
}
