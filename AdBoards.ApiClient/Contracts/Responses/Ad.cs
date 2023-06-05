using System.Text.Json.Serialization;

namespace AdBoards.ApiClient.Contracts.Responses;

public class Ad
{
    public int Id { get; set; }

    public int Price { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string City { get; set; } = null!;

    [JsonPropertyName("photo")] public string PhotoUri { get; set; } = null!;

    public DateOnly Date { get; set; }

    [JsonPropertyName("cotegorysId")] public int CategoryId { get; set; }

    public int PersonId { get; set; }

    [JsonPropertyName("typeOfAdId")] public int TypeOfAdId { get; set; }

    public ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

    [JsonPropertyName("cotegorys")] public Category Category { get; set; } = null!;

    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public Person Person { get; set; } = null!;

    [JsonPropertyName("typeOfAd")] public AdType AdType { get; set; } = null!;
}