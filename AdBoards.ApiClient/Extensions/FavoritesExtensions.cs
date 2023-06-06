using AdBoards.ApiClient.Contracts.Requests;
using AdBoards.ApiClient.Contracts.Responses;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace AdBoards.ApiClient.Extensions;

public static class FavoritesExtensions
{
    public static async Task<bool> AddToFavorites(this AdBoardsApiClient apiClient, int adId)
    {
        using var response = await apiClient.HttpClient.PostAsync($"Favorites/Addition?adId={adId}", new StringContent(""));

        if (response.IsSuccessStatusCode) return true;

        return false;
    }


}