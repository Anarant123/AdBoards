namespace OnlineMarket.ApiClient.Contracts.Responses;

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
        get => $"http://localhost:5228/{_photoName}";
        set => _photoName = value;
    }

    public Role Role { get; set; } = null!;
}