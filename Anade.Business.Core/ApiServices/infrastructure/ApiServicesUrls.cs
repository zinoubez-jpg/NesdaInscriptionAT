using System;

namespace Anade.Business.Core
{
    public class ApiServicesUrls
    {
        private static AppSettings _appSettings;
        public string ApiBaseUrl { get => _appSettings.ApiBaseUrl; }
        public string PathImages { get => _appSettings.PathImages; }

        public ApiServicesUrls(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public Uri CreateRequestUri(string baseUri, string relativePath = "")
        {
            Uri uri = new Uri(baseUri);
            Uri endPoint = new Uri(uri, relativePath);
            UriBuilder uriBuilder = new UriBuilder(endPoint);
            return uriBuilder.Uri;
        }

        public string GetBaseUrl<T>()
        {
            return CreateRequestUri(ApiBaseUrl, $"{typeof(T).Name}").ToString();
        }  
        public string GetBaseUrl<T>(string action)
        {
            return CreateRequestUri(ApiBaseUrl, $"{typeof(T).Name}/{action}").ToString();
        }

        public string GetAll<T>()
        {
            return CreateRequestUri(ApiBaseUrl,$"{typeof(T).Name}/all").ToString();
        }

        public string GetAllPaged<T>()
        {
            return CreateRequestUri(ApiBaseUrl, $"{typeof(T).Name}/paged").ToString();
        }

        public string GetDatabable<T>()
        {
            return CreateRequestUri(ApiBaseUrl, $"{typeof(T).Name}/datatable").ToString();
        }

        public string GetByIdUrl<T>()
        {
            return CreateRequestUri(ApiBaseUrl, $"{typeof(T).Name}/").ToString();
        }
        public string GetByIdUrl<T,TKey>(TKey id, string action = "")
        {
            action += action == "" ? ""  : "/";
            return CreateRequestUri(ApiBaseUrl, $"{typeof(T).Name}/{action}{id}").ToString();
        }       

    }
}
