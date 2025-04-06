namespace AuctionService.Endpoints;

internal sealed class UpdateAuction : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/auctions/{id:guid}",
            async Task<IResult> (Guid id, UpdateAuctionDto updateAuctionDto,
                AuctionDbContext dbContext, IMapper mapper, IPublishEndpoint publishEndpoint) =>
            {
                var auction = await dbContext.Auctions.Include(a => a.Item)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (auction is null) return TypedResults.NotFound();

                // TODO: check seller == username

                auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
                auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
                auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
                auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
                auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

                var updatedAuction = mapper.Map<AuctionDto>(auction);

                await publishEndpoint.Publish(mapper.Map<AuctionUpdated>(updatedAuction));

                var result = await dbContext.SaveChangesAsync() > 0;

                Console.WriteLine($"--> Producing auction updated: {updatedAuction.Id}");

                if (!result) return TypedResults.BadRequest("Unable to save changes to db.");

                return TypedResults.Ok(updatedAuction);
            });
    }
}