using AdBoards.Domain.Auth;

namespace AdBoards.ApiClient.Contracts.Responses;

public class AuthorizedModel
{
    public LoginResult Kind { get; set; }

    public string Token { get; set; } = null!;

    public Person Person { get; set; } = null!;
}