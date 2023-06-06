using System.Text.Json.Serialization;
using AdBoards.Domain.Enums;

namespace AdBoardsWebAPI.Data.Models;

public class Right
{
    public RightType Id { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore] public ICollection<Person> People { get; set; } = new List<Person>();
}