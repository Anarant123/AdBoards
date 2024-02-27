namespace OnlineMarket.WebAPI.Contracts.Requests.Models;

public record UpdatePersonModel
(
    string? Name,
    string? City,
    DateTime? Birthday,
    string? Phone,
    string? Email
);