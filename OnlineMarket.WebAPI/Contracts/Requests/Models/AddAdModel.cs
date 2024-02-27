namespace OnlineMarket.WebAPI.Contracts.Requests.Models;

public record AddAdModel
(
    int Price,
    string Name,
    string Description,
    string City,
    int CategoryId,
    int AdTypeId
);