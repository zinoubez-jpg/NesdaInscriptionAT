using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Anade.Business.Core
{
    public class ApiServiceFactory : IApiServiceFactory
    {
        private readonly Dictionary<Type, object> _apiservices = new Dictionary<Type, object>();
        private readonly ApiServicesUrls _apiServicesUrls;
        private readonly HttpClient _httpClient;

        public ApiServiceFactory(HttpClient httpClient, ApiServicesUrls apiServicesUrls)
        {
            _httpClient = httpClient;
            _apiServicesUrls = apiServicesUrls;
        }

        public IApiService<T> GetApiService<T>() where T : class
        {
            if (!_apiservices.ContainsKey(typeof(T)))
                _apiservices[typeof(T)] = new GenericApiService<T>(_httpClient, _apiServicesUrls);

            return _apiservices[typeof(T)] as IApiService<T>;

        }
    }


}
