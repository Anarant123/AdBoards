using OnlineMarket.Domain.Auth;

namespace OnlineMarket.ApiClient.Contracts.Responses;

public class AuthorizedModel
{
    public LoginResult Kind { get; set; }

    public string Token { get; set; } = null!;

    public Person Person { get; set; } = null!;
}