namespace AuctionService.Endpoints;

internal sealed class GetAuctions : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/auctions",
            async Task<IResult> (AuctionDbContext auctionDbContext, IMapper mapper, string? date) =>
            {
                var query = auctionDbContext.Auctions.OrderBy(a => a.Item.Make).AsQueryable();

                if (!string.IsNullOrEmpty(date))
                    query = query.Where(a => a.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);

                return TypedResults.Ok(await query.ProjectTo<AuctionDto>(mapper.ConfigurationProvider).ToListAsync());
            });
    }
}