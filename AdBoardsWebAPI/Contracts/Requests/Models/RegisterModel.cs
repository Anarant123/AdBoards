namespace AdBoardsWebAPI.Contracts.Requests.Models;

public record RegisterModel
(
    string Login,
    string Password,
    string? Name,
    string? City,
    DateTime Birthday,
    string Phone,
    string Email
);