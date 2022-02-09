using System.Net.Http.Json;
using System.Reflection;
using System.Web;
using IntelligenceReporting.Entities;
using IntelligenceReporting.Queries;

namespace IntelligenceReporting.WebApp.Services
{
    public interface IApiService
    {
        Task<AgentEarningsPagedResults> GetAgentEarnings(AgentEarningsQueryParameters parameters);
        Task<MultiOfficePagedResults> GetMultiOffices(MultiOfficeQueryParameters parameters);
        Task<OfficePagedResults> GetOffices(OfficeQueryParameters parameters);
        Task<StaffPagedResults> GetStaff(StaffQueryParameters parameters);
        Task<StaticData> GetStaticData();
    }


    public class ApiService : IApiService
    {
        private readonly string? _baseApiUrl;
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _baseApiUrl = config["BaseApiUrl"];
        }

        private async Task<TResult> Get<TResult>(string controller)
        {
            var uri = new Uri($"{_baseApiUrl}/api/{controller}");
            var result = await _httpClient.GetFromJsonAsync<TResult>(uri);
            return result!;
        }

        private async Task<TResult> Get<TParameters, TResult>(string controller, TParameters parameters)
            where TParameters : QueryParameters, new()
        {
            var uri = new Uri($"{_baseApiUrl}/api/{controller}?{ToQueryString(parameters)}");
            var result = await _httpClient.GetFromJsonAsync<TResult>(uri);
            return result!;
        }

        private static string ToQueryString<T>(T parameters) where T: QueryParameters, new()
        {
            var defaultParameters = new T();
            var properties = parameters.GetType().GetProperties();
            var multiValParams = GetMultiValueQueryString(properties, parameters);

            var result = string.Join("&",
                properties
                    .Select(p => new { p.Name, Value = GetPropertyValue(parameters, p), DefaultValue = p.GetValue(defaultParameters) })
                    .Where(x => x.Value != x.DefaultValue && !string.IsNullOrEmpty(x.Value?.ToString()))
                    .Select(x => $"{UrlEncode(x.Name)}={x.Value}"));

            result += multiValParams;
            return result;
        }

        private static string GetMultiValueQueryString<T>(PropertyInfo[]? properties, T parameters)
        {
            var multiValParams = string.Empty;

            if(properties != null)
            {
                foreach (var propertyInfo in properties)
                {
                    var value = propertyInfo.GetValue(parameters);

                    if (value is IList<int> && value.GetType().IsGenericType)
                    {
                        foreach (var val in (List<int>)value)
                        {
                            multiValParams += "&" + $"{UrlEncode(propertyInfo.Name)}={val}";
                        }
                    }
                }
            }
            return multiValParams;
        }

        private static object? GetPropertyValue<T>(T parameters, PropertyInfo propertyInfo) where T : QueryParameters, new()
        {
            var value = propertyInfo.GetValue(parameters);

            if(value is IList<int> && value.GetType().IsGenericType)
            {
                return string.Empty;
            }
            
            return UrlEncode(value);
        }

        private static string UrlEncode(object? value)
        {
            if (value is DateTime dateTime)
                return dateTime == dateTime.Date ? $"{dateTime:yyyy-MM-dd}" : $"{dateTime:yyyy-MM-dd HH:mm:ss}";

            return HttpUtility.UrlEncode(value + "");
        }

        public Task<AgentEarningsPagedResults> GetAgentEarnings(AgentEarningsQueryParameters parameters)
            => Get<AgentEarningsQueryParameters, AgentEarningsPagedResults>("AgentEarnings", parameters);

        public Task<MultiOfficePagedResults> GetMultiOffices(MultiOfficeQueryParameters parameters)
            => Get<MultiOfficeQueryParameters, MultiOfficePagedResults>("MultiOffice", parameters);

        public Task<OfficePagedResults> GetOffices(OfficeQueryParameters parameters)
            => Get<OfficeQueryParameters, OfficePagedResults>("Office", parameters);

        public Task<StaffPagedResults> GetStaff(StaffQueryParameters parameters)
            => Get<StaffQueryParameters, StaffPagedResults>("Staff", parameters);

        public Task<StaticData> GetStaticData()
            => Get<StaticData>("StaticData");
    }
}
