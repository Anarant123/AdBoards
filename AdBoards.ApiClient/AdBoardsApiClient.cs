namespace AdBoards.ApiClient;

public class AdBoardsApiClient
{
    internal readonly HttpClient HttpClient;

    public AdBoardsApiClient(string apiBasePath)
    {
        HttpClient = new HttpClient
        {
            BaseAddress = new Uri(apiBasePath)
        };
    }
}