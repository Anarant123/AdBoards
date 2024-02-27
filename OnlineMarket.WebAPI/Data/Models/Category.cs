using System.Text.Json.Serialization;

namespace OnlineMarket.WebAPI.Data.Models;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore] public ICollection<Ad> Ads { get; set; } = new List<Ad>();
}