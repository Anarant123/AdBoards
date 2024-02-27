namespace AdBoardsWebAPI.Contracts.Requests.Models;

public record UpdateAdModel
(
    int Id,
    int? Price,
    string? Name,
    string? Description,
    string? City,
    int? CategoryId,
    int? AdTypeId
);