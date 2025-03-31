namespace AuctionService.Endpoints;

internal sealed class GetAuctionById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/auctions/{id:guid}",
            async Task<IResult> (AuctionDbContext auctionDbContext, Guid id, IMapper mapper) =>
            {
                var auction = await auctionDbContext.Auctions
                    .Include(a => a.Item)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (auction is null) return TypedResults.NotFound();

                return TypedResults.Ok(mapper.Map<AuctionDto>(auction));
            });
    }
}