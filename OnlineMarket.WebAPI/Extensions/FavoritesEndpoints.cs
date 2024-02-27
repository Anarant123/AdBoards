using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.WebAPI.Data;
using OnlineMarket.WebAPI.Data.Models;

namespace OnlineMarket.WebAPI.Extensions;

public static class FavoritesEndpoints
{
    public static IEndpointRouteBuilder MapFavoritesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("Favorites").WithTags("Favorites");

        group.MapGet("IsFavorite", async (int adId, OnlineMarketContext context, ClaimsPrincipal user) =>
        {
            var userId = int.Parse(user.Claims.First(x => x.Type == "id").Value);

            return await context.Favorites.AnyAsync(x => x.AdId == adId && x.PersonId == userId)
                ? Results.Ok()
                : Results.BadRequest();
        });

        group.MapPost("Addition", async (int adId, OnlineMarketContext context, ClaimsPrincipal user) =>
        {
            var userId = int.Parse(user.Claims.First(x => x.Type == "id").Value);

            if (await context.Favorites.AnyAsync(x => x.AdId == adId && x.PersonId == userId))
                return Results.BadRequest();

            var favorite = new Favorite
            {
                AdId = adId,
                PersonId = userId
            };

            context.Favorites.Add(favorite);
            await context.SaveChangesAsync();

            return Results.Ok();
        });

        group.MapDelete("Delete", async (int adId, OnlineMarketContext context, ClaimsPrincipal user) =>
        {
            var userId = int.Parse(user.Claims.First(x => x.Type == "id").Value);

            var ad = await context.Favorites.FirstOrDefaultAsync(x => x.AdId == adId && x.PersonId == userId);
            if (ad is null) return Results.NotFound();

            context.Favorites.Remove(ad);
            await context.SaveChangesAsync();

            return Results.Ok();
        });

        return app;
    }
}