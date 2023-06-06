﻿namespace AdBoards.ApiClient.Contracts.Responses;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Ad> Ads { get; set; } = new List<Ad>();
}