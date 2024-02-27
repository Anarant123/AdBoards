using System.Text.Json.Serialization;

namespace OnlineMarket.WebAPI.Data.Models;

public class Complaint
{
    public int Id { get; set; }

    public int AdId { get; set; }

    public int PersonId { get; set; }

    [JsonIgnore] public virtual Ad Ad { get; set; } = null!;

    [JsonIgnore] public Person Person { get; set; } = null!;
}