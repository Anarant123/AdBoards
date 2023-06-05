namespace AdBoards.ApiClient.Contracts.Responses;

public class AdType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Ad> Ads { get; set; } = new List<Ad>();
}