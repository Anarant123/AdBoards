namespace AdBoards.ApiClient.Extensions;

public static class FavoritesExtensions
{
    public static async Task<bool> AddToFavorites(this AdBoardsApiClient apiClient, int adId)
    {
        using var response =
            await apiClient.HttpClient.PostAsync($"Favorites/Addition?adId={adId}", new StringContent(""));

        if (response.IsSuccessStatusCode) return true;

        return false;
    }

    public static async Task<bool> DeleteFromFavorites(this AdBoardsApiClient apiClient, int adId)
    {
        using var response = await apiClient.HttpClient.DeleteAsync($"Favorites/Delete?adId={adId}");

        if (response.IsSuccessStatusCode) return true;

        return false;
    }
}