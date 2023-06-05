using AdBoardsWebAPI.Contracts.Requests.Models;
using AdBoardsWebAPI.Data;
using AdBoardsWebAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AdBoardsWebAPI.Extensions;

public static class AdEndpoints
{
    public static WebApplication MapAdEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("Ads/");

        group.MapGet("GetAd", async (int id, AdBoardsContext context) =>
        {
            var ad = await context.Ads.Include(x => x.Person).Include(x => x.Complaints)
                .FirstOrDefaultAsync(x => x.Id == id);
            return ad is null ? Results.BadRequest() : Results.Ok(ad);
        });

        group.MapGet("GetAds", async (AdBoardsContext context) =>
        {
            var ads = await context.Ads.Include(x => x.Person).Include(x => x.Complaints).ToListAsync();
            return Results.Ok(ads);
        });

        group.MapGet("GetMyAds", async (int id, AdBoardsContext context) =>
        {
            var ads = await context.Ads.Where(x => x.PersonId == id).ToListAsync();
            return ads.Count != 0 ? Results.Ok(ads) : Results.BadRequest();
        });

        group.MapGet("GetFavoritesAds", async (int id, AdBoardsContext context) =>
        {
            var ads = await context.Favorites
                .Include(x => x.Ad).ThenInclude(x => x.Person)
                .Include(x => x.Ad).ThenInclude(x => x.Complaints)
                .Where(x => x.PersonId == id)
                .Select(x => x.Ad)
                .ToListAsync();

            return ads.Count != 0 ? Results.Ok(ads) : Results.BadRequest();
        });

        group.MapPost("Addition", async (AdModel model, AdBoardsContext context, FileManager fileManager) =>
        {
            var a = new Ad
            {
                Price = model.Price,
                Name = model.Name,
                Description = model.Description,
                City = model.City,
                PhotoName = await fileManager.SaveAdPhoto(model.Photo),
                Date = model.Date,
                CategoryId = model.CotegorysId,
                PersonId = model.PersonId,
                AdTypeId = model.TypeOfAdId
            };

            context.Ads.Add(a);
            await context.SaveChangesAsync();

            return Results.Ok(a);
        });

        group.MapPut("Update", async (UpdateAdModel model, AdBoardsContext context, FileManager fileManager) =>
        {
            var ad = await context.Ads.FindAsync(model.Id);
            if (ad is null) return Results.NotFound();

            if (model.Price is not null) ad.Price = model.Price.Value;
            if (model.Name is not null) ad.Name = model.Name;
            if (model.Description is not null) ad.Description = model.Description;
            if (model.City is not null) ad.City = model.City;
            if (model.Photo is not null) ad.PhotoName = await fileManager.SaveAdPhoto(model.Photo);
            if (model.CotegorysId is not null) ad.CategoryId = model.CotegorysId.Value;
            if (model.TypeOfAdId is not null) ad.AdTypeId = model.TypeOfAdId.Value;

            await context.SaveChangesAsync();

            return Results.Ok(ad);
        });

        group.MapDelete("Delete", async (int id, AdBoardsContext context) =>
        {
            var ad = await context.Ads.FindAsync(id);
            if (ad is null) return Results.NotFound();

            context.Ads.Remove(ad);
            await context.SaveChangesAsync();

            return Results.Ok();
        });

        return app;
    }
}