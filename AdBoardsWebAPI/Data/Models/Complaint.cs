namespace AdBoardsWebAPI.Data.Models;

public class Complaint
{
    public int Id { get; set; }

    public int AdId { get; set; }

    public int PersonId { get; set; }

    public virtual Ad Ad { get; set; } = null!;

    public Person Person { get; set; } = null!;
}