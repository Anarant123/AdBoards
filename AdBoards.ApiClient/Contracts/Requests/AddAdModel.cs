namespace AdBoards.ApiClient.Contracts.Requests;

public class AddAdModel
{
    public decimal Price { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string City { get; set; } = null!;

    public string CategoryId { get; set; } = null!;

    public string AdTypeId { get; set; } = null!;
}