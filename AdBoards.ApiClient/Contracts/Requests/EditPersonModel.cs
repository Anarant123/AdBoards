namespace AdBoards.ApiClient.Contracts.Requests;

public class EditPersonModel
{
    public string? Name { get; set; }

    public string? City { get; set; }

    public DateTime Birthday { get; set; }

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    private string _photoName = null!;

    public string PhotoName
    {
        get => $"https://adboards.site/{_photoName}";
        set => _photoName = value;
    }
}