namespace AdBoards.ApiClient.Contracts.Responses;

public class Right
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Person> People { get; set; } = new List<Person>();
}