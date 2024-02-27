using Newtonsoft.Json;

namespace OnlineMarket.WebAPI.Data.Models;

public class Favorite
{
    public int Id { get; set; }

    [JsonIgnore] public int AdId { get; set; }

    [JsonIgnore] public int PersonId { get; set; }

    public Ad Ad { get; set; } = null!;

    [JsonIgnore] public Person Person { get; set; } = null!;
}