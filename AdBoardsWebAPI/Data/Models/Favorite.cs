namespace AdBoardsWebAPI.Data.Models;

public class Favorite
{
    public int Id { get; set; }

    public int AdId { get; set; }

    public int PersonId { get; set; }

    public Ad Ad { get; set; } = null!;

    public Person Person { get; set; } = null!;
}