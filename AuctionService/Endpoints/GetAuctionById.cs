namespace AuctionService.Endpoints;

internal sealed class GetAuctionById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/auctions/{id:guid}",
            async Task<Results<Ok<AuctionDto>, NotFound>> (AuctionDbContext auctionDbContext, Guid id) =>
            {
                var auctions = await auctionDbContext.Auctions
                    .Include(a => a.Item)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (auctions == null) return TypedResults.NotFound();

                var auctionsDto = new AuctionDto
                {
                    Id = auctions.Id,
                    ReservePrice = auctions.ReservePrice,
                    Seller = auctions.Seller,
                    Winner = auctions.Winner,
                    SoldAmount = auctions.SoldAmount,
                    CurrentHighBid = auctions.CurrentHighBid,
                    CreatedAt = auctions.CreatedAt,
                    UpdatedAt = auctions.UpdatedAt,
                    AuctionEnd = auctions.AuctionEnd,
                    Status = auctions.Status.ToString(),
                    Make = auctions.Item.Make,
                    Model = auctions.Item.Model,
                    Year = auctions.Item.Year,
                    Color = auctions.Item.Color,
                    Mileage = auctions.Item.Mileage,
                    ImageUrl = auctions.Item.ImageUrl
                };

                return TypedResults.Ok(auctionsDto);
            });
    }
}