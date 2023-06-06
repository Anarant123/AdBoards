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

    public static async Task<Ad?> GetAd(this AdBoardsApiClient apiClient, int id)
    {
        using var response = await apiClient.HttpClient.GetAsync($"Ads/GetAd?id={id}");

        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<Ad>();

        return null;
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

        var stream = model.Photo.OpenReadStream();
        var multipart = new MultipartFormDataContent();
        multipart.Add(new StreamContent(stream), "photo", model.Photo.FileName);

        using var response = await apiClient.HttpClient.PutAsync($"Ads/{model.Id}/Photo", multipart);

        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<Ad>();

        return null;
    }
}