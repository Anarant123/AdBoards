using System.Net.Http.Json;
using AdBoards.ApiClient.Contracts.Responses;

namespace AdBoards.ApiClient.Extensions;

public static class AdExtensions
{
    public static async Task<List<Ad>> GetAds(this AdBoardsApiClient apiClient)
    {
        using var response = await apiClient.HttpClient.GetAsync("Ads/GetAds");
        var ads = await response.Content.ReadFromJsonAsync<List<Ad>>();

        return ads!;
    }

    public static async Task AddAd(this AdBoardsApiClient apiClient)
    {
        using var response = await apiClient.HttpClient.GetAsync("Ads/Addition");
        var ads = await response.Content.ReadFromJsonAsync<List<Ad>>();

        return;
    }
}