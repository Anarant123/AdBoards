namespace AdBoards.ApiClient.Contracts.Responses;

public class AuthorizedModel
{
    public string Token { get; set; } = null!;

    public Person Person { get; set; } = null!;
}