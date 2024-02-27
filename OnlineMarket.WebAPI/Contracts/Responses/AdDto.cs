using OnlineMarket.WebAPI.Data.Models;

namespace OnlineMarket.WebAPI.Contracts.Responses;

public class AdDto
{
    public int Id { get; set; }

    public int Price { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string City { get; set; } = null!;

    public string PhotoName { get; set; } = null!;

    public DateOnly Date { get; set; }

    public bool IsFavorite { get; set; }

    public AdType AdType { get; set; } = null!;

    public Category Category { get; set; } = null!;

    public ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

    public Person Person { get; set; } = null!;

    public static AdDto Map(Ad ad, bool isFavorite)
    {
        return new AdDto
        {
            Id = ad.Id,
            Price = ad.Price,
            Name = ad.Name,
            Description = ad.Description,
            City = ad.City,
            PhotoName = ad.PhotoName,
            Date = ad.Date,
            IsFavorite = isFavorite,
            AdType = ad.AdType,
            Category = ad.Category,
            Complaints = ad.Complaints,
            Person = ad.Person
        };
    }
}