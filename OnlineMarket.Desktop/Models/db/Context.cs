using OnlineMarket.ApiClient;
using OnlineMarket.ApiClient.Contracts.Responses;
using System;
using System.Collections.Generic;

namespace OnlineMarket.Desktop.Models.db;

public partial class Context 
{
    public static OnlineMarketApiClient Api = new OnlineMarketApiClient("http://localhost:5228/api/");
    public static AuthorizedModel? UserNow { get; set; }
    public static Ad? AdNow { get; set; }
    public static AdListViewModel? AdList { get; set; } = null;
}
