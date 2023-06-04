using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AdBoardsWebAPI.Models.db;

public partial class Person
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("login")]
    public string? Login { get; set; }

    [JsonIgnore]
    public string? Password { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("birthday")]
    public DateTime? Birthday { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("photo")]
    public byte[]? Photo { get; set; }

    [JsonPropertyName("rightId")]
    public int? RightId { get; set; }

    [JsonPropertyName("ads")]
    public virtual ICollection<Ad> Ads { get; set; } = new List<Ad>();

    [JsonPropertyName("complaints")]
    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

    [JsonPropertyName("favorites")]
    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    [JsonPropertyName("right")]
    public virtual Right? Right { get; set; }
}
