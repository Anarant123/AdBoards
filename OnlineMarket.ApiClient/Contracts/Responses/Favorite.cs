namespace OnlineMarket.ApiClient.Contracts.Responses;

public class Favorite
{
    public int Id { get; set; }

    public Ad Ad { get; set; } = null!;
}