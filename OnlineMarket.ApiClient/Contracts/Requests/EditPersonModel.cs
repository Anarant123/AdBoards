using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using OnlineMarket.ApiClient.Contracts.Responses;

namespace OnlineMarket.ApiClient.Contracts.Requests;

public class EditPersonModel
{
    public string? Name { get; set; }

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime Birthday { get; set; }

    public string? City { get; set; }

    [JsonIgnore] public string PhotoName { get; set; } = string.Empty;

    [JsonIgnore] public IFormFile? Photo { get; set; }

    public static EditPersonModel MapFromPerson(Person person)
    {
        return new EditPersonModel
        {
            Name = person.Name,
            Phone = person.Phone,
            Email = person.Email,
            Birthday = person.Birthday,
            City = person.City,
            PhotoName = person.PhotoName
        };
    }
}