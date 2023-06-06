using System.Text.Json.Serialization;

namespace AdBoards.ApiClient.Contracts.Responses;

public class Person
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string? Name { get; set; }

    public string? City { get; set; }

    public DateOnly Birthday { get; set; }

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    private string _photoName = null!;

    public string PhotoName
    {
        get => $"https://adboards.site/{_photoName}";
        set => _photoName = value;
    }

    public Right Right { get; set; } = null!;
}