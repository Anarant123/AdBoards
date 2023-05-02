using System;
using System.Collections.Generic;

namespace AdBoardsWebAPI.Models.db;

public partial class Ad
{
    public int Id { get; set; }

    public int? Price { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? City { get; set; }

    public byte[]? Photo { get; set; }

    public DateTime? Date { get; set; }

    public int? CotegorysId { get; set; }

    public int? PersonId { get; set; }

    public int? TypeOfAdId { get; set; }

    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

    public virtual Category? Cotegorys { get; set; }

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual Person? Person { get; set; }

    public virtual TypeOfAd? TypeOfAd { get; set; }
}
