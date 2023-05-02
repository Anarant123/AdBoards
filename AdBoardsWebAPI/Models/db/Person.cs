using System;
using System.Collections.Generic;

namespace AdBoardsWebAPI.Models.db;

public partial class Person
{
    public int Id { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public string? Name { get; set; }

    public string? City { get; set; }

    public DateTime? Birthday { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public byte[]? Photo { get; set; }

    public int? RightId { get; set; }

    public virtual ICollection<Ad> Ads { get; set; } = new List<Ad>();

    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual Right? Right { get; set; }
}
