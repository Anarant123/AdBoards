namespace AdBoards.ApiClient.Contracts.Responses;

public class Person
{
    private string _photoName = null!;
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string? Name { get; set; }

    public string? City { get; set; }

    public DateTime Birthday { get; set; }

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhotoName
    {
        get => $"https://adboards.site/{_photoName}";
        set => _photoName = value;
    }

    public Right Right { get; set; } = null!;
}