using Refit;

namespace HavaPraqnoz.Services.Abstractions;

public interface IWeatherClient
{
    [Get("/search.json?key={key}&q={query}")]
    Task<dynamic> SearchAsync(string key, string query);
    [Get("/forecast.json?key={key}&q=id:{id}&days=3&aqi=no&alerts=no")]
    Task<dynamic> GetAsync(string key, int id);
}
