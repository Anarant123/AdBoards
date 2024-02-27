using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.WebAPI.Auth;
using OnlineMarket.WebAPI.Data;
using OnlineMarket.WebAPI.Data.Models;

namespace OnlineMarket.WebAPI.Extensions;

public static class ComplaintEndpoints
{
    public static IEndpointRouteBuilder MapComplaintEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("Complaint/").WithTags("Complaint");

        group.MapPost("Addition", async (int adId, OnlineMarketContext context, ClaimsPrincipal user) =>
        {
            var userId = int.Parse(user.Claims.First(x => x.Type == "id").Value);

            if (await context.Complaints.AnyAsync(x => x.AdId == adId && x.PersonId == userId))
                return Results.BadRequest();

            var c = new Complaint
            {
                AdId = adId,
                PersonId = userId
            };

            context.Complaints.Add(c);
            await context.SaveChangesAsync();

            return Results.Ok();
        });

        group.MapDelete("Delete", async (int adId, OnlineMarketContext context) =>
        {
            var complaints = context.Complaints.Where(x => x.AdId == adId);

            foreach (var c in complaints) context.Complaints.Remove(c);

            await context.SaveChangesAsync();

            return Results.Ok();
        }).RequireAuthorization(Policies.Admin);

        return app;
    }
}