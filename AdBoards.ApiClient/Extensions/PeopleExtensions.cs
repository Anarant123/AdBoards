using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
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

    public static async Task<bool> Registr(this AdBoardsApiClient api, PersonReg person)
    {
        if (person.Password != person.ConfirmPassword)
        {
            return false;
        }

        using var jsonContent = new StringContent(JsonSerializer.Serialize(person), Encoding.UTF8, "application/json");
        using var response = await api.HttpClient.PostAsync($"People/Registration", jsonContent);

        if(response.IsSuccessStatusCode)
            return true;

        return false;
    }

    public static async Task Recover(this AdBoardsApiClient api, string Login)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"People/RecoveryPassword?Login={Login}");
    }

    public static async Task<Person?> GetMe(this AdBoardsApiClient api)
    {
        using var response = await api.HttpClient.GetAsync("People/GetMe");
        return await response.Content.ReadFromJsonAsync<Person>();
    }
}