using AdBoardsWebAPI.DomainTypes.Enums;

namespace AdBoardsWebAPI.Data.Models;

public class Right
{
    public RightType Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<Person> People { get; set; } = new List<Person>();
}