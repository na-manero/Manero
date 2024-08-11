using System.Text.Json;
using System.Text;
using System.Net;

namespace Manero.Web.Helpers;

public class ApiHelper(HttpClient client, TimeSpan? retryDelay = null) : IApiHelper
{
    private readonly HttpClient _client = client;
    private readonly TimeSpan _retryDelay = retryDelay ?? TimeSpan.FromSeconds(2);

    private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public async Task<HttpResponseMessage> PostAsync<T>(string url, T model)
    {
        var jsonContent = JsonSerializer.Serialize(model, _jsonSerializerOptions);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        return await ExecuteWithRetry(() => _client.PostAsync(url, content));
    }

    public async Task<T> GetAsync<T>(string url)
    {
        var response = await ExecuteWithRetry(() => _client.GetAsync(url));

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(jsonResponse, _jsonSerializerOptions)!;
        }

        return default!;
    }

    private async Task<HttpResponseMessage> ExecuteWithRetry(Func<Task<HttpResponseMessage>> operation)
    {
        try
        {
            return await operation();
        }
        catch (HttpRequestException)
        {
            await Task.Delay(_retryDelay);
            try
            {
                return await operation();
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Internal server error. Please try again later.")
                };
            }
        }
    }
}