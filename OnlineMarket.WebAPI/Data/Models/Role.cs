using System.Text.Json.Serialization;
using OnlineMarket.Domain.Enums;

namespace OnlineMarket.WebAPI.Data.Models;

public class Role
{
    public RoleType Id { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore] public ICollection<Person> People { get; set; } = new List<Person>();
}