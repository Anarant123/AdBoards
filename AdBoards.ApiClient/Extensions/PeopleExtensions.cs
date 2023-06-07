using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using AdBoards.ApiClient.Contracts.Requests;
using AdBoards.ApiClient.Contracts.Responses;

namespace AdBoards.ApiClient.Extensions;

public static class PeopleExtensions
{
    public static async Task<AuthorizedModel?> Authorize(this AdBoardsApiClient api, string login, string password)
    {
        using var response = await api.HttpClient.GetAsync($"People/Authorization?login={login}&password={password}");

        try
        {
            var model = await response.Content.ReadFromJsonAsync<AuthorizedModel>();
            return model;
        }
        catch
        {
            return null;
        }
    }

    public static async Task<bool> Registr(this AdBoardsApiClient api, PersonReg person)
    {
        if (person.Password != person.ConfirmPassword) return false;

        using var jsonContent = new StringContent(JsonSerializer.Serialize(person), Encoding.UTF8, "application/json");
        using var response = await api.HttpClient.PostAsync("People/Registration", jsonContent);

        return response.IsSuccessStatusCode;
    }

    public static async Task Recover(this AdBoardsApiClient api, string login)
    {
        using var response = await api.HttpClient.PostAsync($"People/RecoveryPassword?login={login}",
            new StringContent(""));
    }

    public static async Task<Person?> GetMe(this AdBoardsApiClient api)
    {
        using var response = await api.HttpClient.GetAsync("People/GetMe");
        return await response.Content.ReadFromJsonAsync<Person>();
    }

    public static async Task<Person?> PersonUpdate(this AdBoardsApiClient api, EditPersonModel person)
    {
        using var jsonContent = new StringContent(JsonSerializer.Serialize(person), Encoding.UTF8, "application/json");
        using var response = await api.HttpClient.PutAsync("People/Update", jsonContent);

        try
        {
            var model = await response.Content.ReadFromJsonAsync<Person>();
            return model;
        }
        catch
        {
            return null;
        }
    }

    public static async Task<Person?> UpdatePersonPhoto(this AdBoardsApiClient api, EditPersonModel model)
    {
        if (model.Photo is null) return null;

        var stream = model.Photo.OpenReadStream();
        var multipart = new MultipartFormDataContent
        {
            { new StreamContent(stream), "photo", model.Photo.FileName }
        };

        using var response = await api.HttpClient.PutAsync("People/Photo", multipart);

        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<Person>();

        return null;
    }

    public static async Task<int> GetCountOfClient(this AdBoardsApiClient api)
    {
        using var response = await api.HttpClient.GetAsync("People/GetCountOfClient");

        var count = Convert.ToInt32(await response.Content.ReadAsStringAsync());
        return count;
    }

    public static async Task<List<Person>?> GetPeople(this AdBoardsApiClient api)
    {
        using var response = await api.HttpClient.GetAsync("People/GetPeople");
        return await response.Content.ReadFromJsonAsync<List<Person>>();
    }

    public static async Task<bool> DeletePeople(this AdBoardsApiClient api, string login)
    {
        using var response = await api.HttpClient.DeleteAsync($"People/Delete?login={login}");
        return response.IsSuccessStatusCode;
    }
}