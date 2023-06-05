using AdBoardsWebAPI.Data;
using AdBoardsWebAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AdBoardsWebAPI.Extensions;

public static class ComplaintEndpoints
{
    public static WebApplication MapComplaintEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("Complaint/");

        group.MapPost("Addition", async (int adId, int personId, AdBoardsContext context) =>
        {
            if (await context.Complaints.AnyAsync(x => x.AdId == adId && x.PersonId == personId))
                return Results.BadRequest();

            var c = new Complaint
            {
                AdId = adId,
                PersonId = personId
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
        });

        return app;
    }
}