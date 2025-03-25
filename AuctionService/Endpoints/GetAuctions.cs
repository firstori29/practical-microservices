namespace AuctionService.Endpoints;

internal sealed class GetAuctions : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/auctions",
            async Task<IResult> (AuctionDbContext auctionDbContext) =>
            {
                var auctions = await auctionDbContext.Auctions
                    .Include(a => a.Item)
                    .OrderBy(a => a.Item.Make)
                    .ToListAsync();

                var auctionsDto = auctions.Select(a => new AuctionDto
                {
                    Id = a.Id,
                    ReservePrice = a.ReservePrice,
                    Seller = a.Seller,
                    Winner = a.Winner,
                    SoldAmount = a.SoldAmount,
                    CurrentHighBid = a.CurrentHighBid,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    AuctionEnd = a.AuctionEnd,
                    Status = a.Status.ToString(),
                    Make = a.Item.Make,
                    Model = a.Item.Model,
                    Year = a.Item.Year,
                    Color = a.Item.Color,
                    Mileage = a.Item.Mileage,
                    ImageUrl = a.Item.ImageUrl
                }).ToList();

                return TypedResults.Ok(auctionsDto);
            });
    }
}