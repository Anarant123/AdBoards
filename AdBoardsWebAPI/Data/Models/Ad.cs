using System.Text.Json.Serialization;

namespace AdBoardsWebAPI.Data.Models;

public class Ad
{
    public int Id { get; set; }

    public int Price { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string City { get; set; } = null!;

    public string PhotoName { get; set; } = null!;

    public DateOnly Date { get; set; }

    [JsonIgnore] public int CategoryId { get; set; }

    [JsonIgnore] public int PersonId { get; set; }

    [JsonIgnore] public int AdTypeId { get; set; }

    public AdType AdType { get; set; } = null!;

    public Category Category { get; set; } = null!;

    public ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

    [JsonIgnore] public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public Person Person { get; set; } = null!;
}