using System.Security.Claims;
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

        group.MapPost("Addition", async (int adId, AdBoardsContext context, ClaimsPrincipal user) =>
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