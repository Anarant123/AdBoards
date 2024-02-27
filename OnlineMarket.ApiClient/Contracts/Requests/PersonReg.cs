namespace OnlineMarket.ApiClient.Contracts.Requests;

public class PersonReg
{
    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string ConfirmPassword { get; set; } = null!;

    public DateTime Birthday { get; set; }

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;
}