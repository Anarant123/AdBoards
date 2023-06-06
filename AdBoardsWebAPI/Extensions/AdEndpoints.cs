using System.Security.Claims;
using AdBoards.Domain.Enums;
using AdBoardsWebAPI.Contracts.Requests.Models;
using AdBoardsWebAPI.Data;
using AdBoardsWebAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AdBoardsWebAPI.Extensions;

public static class AdEndpoints
{
    public static WebApplication MapAdEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("Ads/").WithTags("Ads");

        group.MapGet("GetAd", async (int id, AdBoardsContext context) =>
        {
            var ad = await context.Ads
                .Include(x => x.Complaints)
                .Include(x => x.Category)
                .Include(x => x.Person)
                .Include(x => x.AdType)
                .FirstOrDefaultAsync(x => x.Id == id);
            return ad is null ? Results.BadRequest() : Results.Ok(ad);
        }).AllowAnonymous();

        group.MapGet("GetAds", async (AdBoardsContext context) =>
        {
            var ads = await context.Ads
                .Include(x => x.Complaints)
                .Include(x => x.Category)
                .Include(x => x.Person)
                .Include(x => x.AdType)
                .ToListAsync();
            return Results.Ok(ads);
        }).AllowAnonymous();

        group.MapGet("GetMyAds", async (AdBoardsContext context, ClaimsPrincipal user) =>
        {
            var userId = int.Parse(user.Claims.First(x => x.Type == "id").Value);
            var ads = await context.Ads
                .Include(x => x.Complaints)
                .Include(x => x.Category)
                .Include(x => x.Person)
                .Include(x => x.AdType)
                .Where(x => x.PersonId == userId)
                .ToListAsync();

            return Results.Ok(ads);
        });

        group.MapGet("GetFavoritesAds", async (AdBoardsContext context, ClaimsPrincipal user) =>
        {
            var userId = int.Parse(user.Claims.First(x => x.Type == "id").Value);
            var ads = await context.Favorites
                .Include(x => x.Ad).ThenInclude(x => x.Complaints)
                .Include(x => x.Ad).ThenInclude(x => x.Category)
                .Include(x => x.Ad).ThenInclude(x => x.Person)
                .Include(x => x.Ad).ThenInclude(x => x.AdType)
                .Where(x => x.PersonId == userId)
                .Select(x => x.Ad)
                .ToListAsync();

            return Results.Ok(ads);
        });

        group.MapPost("Addition", async (AddAdModel model, AdBoardsContext context, ClaimsPrincipal user,
            FileManager fileManager) =>
        {
            var userId = int.Parse(user.Claims.First(x => x.Type == "id").Value);
            var a = new Ad
            {
                Price = model.Price,
                Name = model.Name,
                Description = model.Description,
                City = model.City,
                Date = DateOnly.FromDateTime(DateTime.Today),
                CategoryId = model.CategoryId,
                PersonId = userId,
                AdTypeId = model.AdTypeId,
                PhotoName = await fileManager.SaveAdPhoto(null)
            };

            context.Ads.Add(a);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
                return Results.Conflict();
            }

            return Results.Ok(a);
        });

        group.MapPut("{id:int}/Photo", async (int id, IFormFile? photo, AdBoardsContext context,
            FileManager fileManager, ClaimsPrincipal user) =>
        {
            var userId = int.Parse(user.Claims.First(x => x.Type == "id").Value);
            var ad = await context.Ads
                .Include(x => x.Complaints)
                .Include(x => x.Category)
                .Include(x => x.Person)
                .Include(x => x.AdType)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (ad is null) return Results.NotFound();

            if (ad.PersonId != userId) return Results.Forbid();

            ad.PhotoName = await fileManager.SaveAdPhoto(photo);

            await context.SaveChangesAsync();

            return Results.Ok(ad);
        });

        group.MapPut("Update", async (UpdateAdModel model, AdBoardsContext context, ClaimsPrincipal user) =>
        {
            var userId = int.Parse(user.Claims.First(x => x.Type == "id").Value);
            var ad = await context.Ads
                .Include(x => x.Complaints)
                .Include(x => x.Category)
                .Include(x => x.Person)
                .Include(x => x.AdType)
                .FirstOrDefaultAsync(x => x.Id == model.Id);
            if (ad is null) return Results.NotFound();

            if (ad.PersonId != userId) return Results.Forbid();

            if (model.Price is not null) ad.Price = model.Price.Value;
            if (model.Name is not null) ad.Name = model.Name;
            if (model.Description is not null) ad.Description = model.Description;
            if (model.City is not null) ad.City = model.City;
            if (model.CotegorysId is not null) ad.CategoryId = model.CotegorysId.Value;
            if (model.TypeOfAdId is not null) ad.AdTypeId = model.TypeOfAdId.Value;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
                return Results.Conflict();
            }

            return Results.Ok(ad);
        });

        group.MapDelete("Delete", async (int id, AdBoardsContext context, ClaimsPrincipal user) =>
        {
            var userId = int.Parse(user.Claims.First(x => x.Type == "id").Value);
            var userRightId = int.Parse(user.Claims.First(x => x.Type == "rightId").Value);

            var ad = await context.Ads.FindAsync(id);
            if (ad is null) return Results.NotFound();

            if (userRightId != (int)RightType.Admin || userId != ad.PersonId) return Results.Forbid();

            context.Ads.Remove(ad);
            await context.SaveChangesAsync();

            return Results.Ok();
        });

        return app;
    }
}