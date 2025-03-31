namespace AuctionService.Endpoints;

internal sealed class CreateAuction : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/auctions",
            async Task<IResult> (CreateAuctionDto createAuctionDto,
                AuctionDbContext dbContext, IMapper mapper, IPublishEndpoint publishEndpoint) =>
            {
                var auction = mapper.Map<Auction>(createAuctionDto);

                // TODO: Add current user as seller.
                auction.Seller = "test";

                await dbContext.Auctions.AddAsync(auction);

                var newAuction = mapper.Map<AuctionDto>(auction);

                await publishEndpoint.Publish(mapper.Map<AuctionCreated>(newAuction));

                var result = await dbContext.SaveChangesAsync() > 0;

                Console.WriteLine($"--> Producing auction created: {newAuction.Id}");

                if (!result) return TypedResults.BadRequest();

                return TypedResults.Created("api/auctions", newAuction);
            });
    }
}