using System.Security.Claims;
using AdBoardsWebAPI.Auth;
using AdBoardsWebAPI.Data;
using AdBoardsWebAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AdBoardsWebAPI.Extensions;

public static class ComplaintEndpoints
{
    public static WebApplication MapComplaintEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("Complaint/").WithTags("Complaint");

        group.MapPost("Addition", async (int adId, AdBoardsContext context, ClaimsPrincipal user) =>
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

        group.MapDelete("Delete", async (int adId, AdBoardsContext context) =>
        {
            var complaints = context.Complaints.Where(x => x.AdId == adId);

            foreach (var c in complaints) context.Complaints.Remove(c);

            await context.SaveChangesAsync();

            return Results.Ok();
        }).RequireAuthorization(Policies.Admin);

        return app;
    }
}