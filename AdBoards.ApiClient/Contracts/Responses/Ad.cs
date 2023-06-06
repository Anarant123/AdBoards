using System.Text.Json.Serialization;

namespace AdBoards.ApiClient.Contracts.Responses;

public class Ad
{
    public int Id { get; set; }

    public int Price { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string City { get; set; } = null!;

    private string _photoName = null!;

    public string PhotoName
    {
        get => "https://adboards.site/" + _photoName;
        set => _photoName = value;
    }

    public DateOnly Date { get; set; }

    public IEnumerable<Complaint> Complaints { get; set; } = Enumerable.Empty<Complaint>();

    public Category Category { get; set; } = null!;

    public Person Person { get; set; } = null!;

    public AdType AdType { get; set; } = null!;
}