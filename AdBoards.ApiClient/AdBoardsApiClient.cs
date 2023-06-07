using System.Net.Http.Headers;

namespace AdBoards.ApiClient;

public class AdBoardsApiClient
{
    internal readonly HttpClient HttpClient;

    private string? _jwt;

    public AdBoardsApiClient(string apiBasePath)
    {
        HttpClient = new HttpClient
        {
            BaseAddress = new Uri(apiBasePath)
        };
        Jwt = null;
    }

    public string? Jwt
    {
        get => _jwt;
        set
        {
            _jwt = value;
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwt);
        }
    }
}