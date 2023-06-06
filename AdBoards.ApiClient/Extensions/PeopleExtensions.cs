using System.Net.Http.Json;
using AdBoards.ApiClient.Contracts.Responses;

namespace AdBoards.ApiClient.Extensions;

public static class PeopleExtensions
{
    public static async Task<AuthorizedModel?> Authorize(this AdBoardsApiClient api, string login, string password)
    {
        using var response = await api.HttpClient.GetAsync($"People/Authorization?login={login}&password={password}");
        var model = await response.Content.ReadFromJsonAsync<AuthorizedModel>();

        return model;
    }

    public static async Task<Person?> GetMe(this AdBoardsApiClient api)
    {
        using var response = await api.HttpClient.GetAsync("People/GetMe");
        return await response.Content.ReadFromJsonAsync<Person>();
    }
}