namespace AdBoards.ApiClient.Extensions;

public static class ComplaintExtensions
{
    public static async Task<bool> AddToComplaints(this AdBoardsApiClient apiClient, int adId)
    {
        using var response =
            await apiClient.HttpClient.PostAsync($"Complaint/Addition?adId={adId}", new StringContent(""));

        if (response.IsSuccessStatusCode) return true;

        return false;
    }

    public static async Task<bool> DeleteFromComplaints(this AdBoardsApiClient apiClient, int adId)
    {
        using var response = await apiClient.HttpClient.DeleteAsync($"Complaint/Delete?adId={adId}");

        if (response.IsSuccessStatusCode) return true;

        return false;
    }
}