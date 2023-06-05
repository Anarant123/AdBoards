using AdBoardsWebAPI.Data;
using AdBoardsWebAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AdBoardsWebAPI.Extensions;

public static class FavoritesEndpoints
{
    public static WebApplication MapFavoritesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("Favorites");

        group.MapGet("IsFavorite", async (int adId, int personId, AdBoardsContext context) =>
        {
            return await context.Favorites.FirstOrDefaultAsync(x => x.AdId == adId && x.PersonId == personId)
                is not null
                ? Results.Ok()
                : Results.BadRequest();
        });

        group.MapPost("Addition", async (int adId, int personId, AdBoardsContext context) =>
        {
            if (await context.Favorites.AnyAsync(x => x.AdId == adId && x.PersonId == personId))
                return Results.BadRequest();

            var f = new Favorite
            {
                AdId = adId,
                PersonId = personId
            };

            context.Favorites.Add(f);
            await context.SaveChangesAsync();

            return Results.Ok();
        });

        group.MapDelete("Delete", async (int adId, int personId, AdBoardsContext context) =>
        {
            var ad = await context.Favorites.FirstOrDefaultAsync(x => x.AdId == adId && x.PersonId == personId);
            if (ad is null) return Results.NotFound();

            context.Favorites.Remove(ad);
            await context.SaveChangesAsync();

            return Results.Ok();
        });

        return app;
    }
}