namespace AuctionService.Endpoints;

internal sealed class UpdateAuction : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/auctions/{id:guid}",
            async Task<IResult> (Guid id, updateAuctionDto updateAuctionDto, AuctionDbContext dbContext) =>
            {
                var auction = await dbContext.Auctions.Include(a => a.Item).FirstOrDefaultAsync(a => a.Id == id);

                if (auction is null) return TypedResults.NotFound();

                // todo: check seller == username

                auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
                auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
                auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
                auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
                auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

                var result = await dbContext.SaveChangesAsync() > 0;

                if (!result) return TypedResults.BadRequest("Unable to save changes to db.");

                return TypedResults.Ok(auction);
            });
    }
}