namespace AuctionService.Endpoints;

internal sealed class DeleteAuction : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/auctions/{id:guid}", async Task<IResult> (Guid id, AuctionDbContext dbContext) =>
        {
            var auction = await dbContext.Auctions.FindAsync(id);

            if (auction is null) return Results.NotFound();

            dbContext.Auctions.Remove(auction);

            var result = await dbContext.SaveChangesAsync() > 0;

            return !result ? Results.BadRequest("Cannot delete auction") : TypedResults.Ok();
        });
    }
}