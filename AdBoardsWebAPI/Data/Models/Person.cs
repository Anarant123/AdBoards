using AdBoardsWebAPI.DomainTypes.Enums;

namespace AdBoardsWebAPI.Data.Models;

public class Person
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Name { get; set; }

    public string? City { get; set; }

    public DateOnly Birthday { get; set; }

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhotoName { get; set; } = null!;

    public RightType RightId { get; set; }

    public ICollection<Ad> Ads { get; set; } = new List<Ad>();

    public ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public Right Right { get; set; } = null!;
}