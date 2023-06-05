namespace AdBoardsWebAPI.Contracts.Requests.Models;

public record AdModel
(
    int Id,
    int Price,
    string Name,
    string Description,
    string City,
    IFormFile? Photo,
    DateOnly Date,
    int CotegorysId,
    int PersonId,
    int TypeOfAdId
);