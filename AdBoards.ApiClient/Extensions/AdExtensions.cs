using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using AdBoards.ApiClient.Contracts.Requests;
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

    public static async Task<Ad?> AddAd(this AdBoardsApiClient apiClient, AddAdModel model)
    {
        using var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
        using var response = await apiClient.HttpClient.PostAsync("Ads/Addition", jsonContent);

        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<Ad>();

        return null;
    }

    public static async Task<Ad?> UpdateAdPhoto(this AdBoardsApiClient apiClient, AddAdModel model)
    {
        if (model.Photo is null) return null;

        using var content = new StreamContent(model.Photo.OpenReadStream());
        using var response = await apiClient.HttpClient.PostAsync($"Ads/{model.Id}/Photo", content);

        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<Ad>();

        return null;
    }
}