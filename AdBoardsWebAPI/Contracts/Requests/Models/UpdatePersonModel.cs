namespace AdBoardsWebAPI.Contracts.Requests.Models;

public record UpdatePersonModel
(
    string? Name,
    string? City,
    DateOnly? Birthday,
    string? Phone,
    string? Email
);