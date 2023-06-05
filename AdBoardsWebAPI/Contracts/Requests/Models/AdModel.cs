namespace AdBoardsWebAPI.Contracts.Requests.Models;

public record AdModel
(
    int Price,
    string Name,
    string Description,
    string City,
    int CotegorysId,
    int TypeOfAdId
);