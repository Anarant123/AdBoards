namespace AdBoardsWebAPI.Data.Models;

public class AdType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<Ad> Ads { get; set; } = new List<Ad>();
}