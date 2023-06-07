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

    public static async Task<List<Ad>> GetMyAds(this AdBoardsApiClient apiClient)
    {
        using var response = await apiClient.HttpClient.GetAsync("Ads/GetMyAds");
        var ads = await response.Content.ReadFromJsonAsync<List<Ad>>();

        return ads!;
    }

    public static async Task<List<Ad>> GetFavoritesAds(this AdBoardsApiClient apiClient)
    {
        using var response = await apiClient.HttpClient.GetAsync("Ads/GetFavoritesAds");
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

    public static async Task<bool> DeleteAd(this AdBoardsApiClient apiClient, int adId)
    {
        using var response = await apiClient.HttpClient.DeleteAsync($"Ads/Delete?id={adId}");

        return response.IsSuccessStatusCode;
    }

    public static async Task<Ad?> AdUpdate(this AdBoardsApiClient api, AddAdModel ad)
    {
        using var jsonContent = new StringContent(JsonSerializer.Serialize(ad), Encoding.UTF8, "application/json");
        using var response = await api.HttpClient.PutAsync("Ads/Update", jsonContent);

        try
        {
            var model = await response.Content.ReadFromJsonAsync<Ad>();
            return model;
        }
        catch
        {
            return null;
        }
    }

    public static async Task<Ad?> UpdateAdPhoto(this AdBoardsApiClient apiClient, AddAdModel model)
    {
        if (model.Photo is null) return null;

        var stream = model.Photo.OpenReadStream();
        var multipart = new MultipartFormDataContent
        {
            { new StreamContent(stream), "photo", model.Photo.FileName }
        };

        using var response = await apiClient.HttpClient.PutAsync($"Ads/{model.Id}/Photo", multipart);

        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<Ad>();

        return null;
    }

    public static async Task<List<Ad>> UseFulter(this AdBoardsApiClient apiClient, int type, string? priceFrom,
        string? priceUpTo, string? city, int categoryId, bool rbBuy, bool rbSell)
    {
        var ads = new List<Ad>();
        switch (type)
        {
            case 1:
                ads = await apiClient.GetAds();
                break;
            case 2:
                ads = await apiClient.GetMyAds();
                break;
            case 3:
                ads = await apiClient.GetFavoritesAds();
                break;
        }

        if (!string.IsNullOrEmpty(priceFrom))
            ads = ads.Where(x => x.Price >= Convert.ToInt32(priceFrom)).ToList();
        if (!string.IsNullOrEmpty(priceUpTo))
            ads = ads.Where(x => x.Price <= Convert.ToInt32(priceUpTo)).ToList();
        if (!string.IsNullOrEmpty(city))
            ads = ads.Where(x => x.City == city).ToList();
        if (categoryId != 0)
            ads = ads.Where(x => x.Category.Id == categoryId).ToList();
        if (Convert.ToBoolean(rbBuy))
            ads = ads.Where(x => x.AdType.Id == 1).ToList();
        else if (Convert.ToBoolean(rbSell))
            ads = ads.Where(x => x.AdType.Id == 2).ToList();

        return ads;
    }
}