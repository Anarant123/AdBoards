using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;


namespace AdBoards.ApiClient.Contracts.Requests;

public class AddAdModel
{
    public int Id { get; set; }

    public int Price { get; set; }

    public string? Name { get; set; }

    public string Description { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public int AdTypeId { get; set; }

    public string? PhotoName { get; set; }

    [JsonIgnore] public IFormFile? Photo { get; set; }
}