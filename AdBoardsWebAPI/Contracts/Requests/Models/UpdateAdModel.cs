namespace AdBoardsWebAPI.Contracts.Requests.Models;

public record UpdateAdModel
(
    int Id,
    int? Price,
    string? Name,
    string? Description,
    string? City,
    IFormFile? Photo,
    DateOnly? Date,
    int? CotegorysId,
    int? TypeOfAdId
);