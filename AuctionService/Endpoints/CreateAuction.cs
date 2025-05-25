namespace AuctionService.Endpoints;

internal sealed class CreateAuction(IHttpContextAccessor httpContextAccessor) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/auctions",
            [Authorize] async Task<IResult> (CreateAuctionDto createAuctionDto,
                AuctionDbContext dbContext, IMapper mapper, IPublishEndpoint publishEndpoint) =>
            {
                var auction = mapper.Map<Auction>(createAuctionDto);
                
                auction.Seller = httpContextAccessor.HttpContext?.User.Identity?.Name!;

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