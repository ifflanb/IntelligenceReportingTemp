using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace IntelligenceReporting.WebApp.Services
{
    public interface ILocalStorageService
    {
        Task<T> Get<T>() where T : new();
        Task Set<T>(T value);
    }

    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime _js;

        public LocalStorageService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<T> Get<T>() where T : new()
        {
            var json = await GetFromLocalStorage(typeof(T).Name);
            var result = JsonConvert.DeserializeObject<T>(json);
            return result ?? new T();
        }

        public async Task<string> GetFromLocalStorage(string key)
        {
            return await _js.InvokeAsync<string>("localStorage.getItem", key);
        }

        public async Task Set<T>(T value)
        {
            var json = JsonConvert.SerializeObject(value);
            await SetLocalStorage(typeof(T).Name, json);
        }

        public async Task SetLocalStorage(string key, string value)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", key, value);
        }
    }
}
