namespace AuctionService.Endpoints;

internal sealed class CreateAuction : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/auctions",
            async Task<Results<Created<AuctionDto>, BadRequest>> (CreateAuctionDto request,
                AuctionDbContext dbContext) =>
            {
                var auctionsDto = new AuctionDto();

                var auction = new Auction
                {
                    ReservePrice = request.ReservePrice,
                    AuctionEnd = request.AuctionEnd,
                    Item = new Item
                    {
                        Make = request.Make,
                        Model = request.Model,
                        Year = request.Year,
                        Color = request.Color,
                        Mileage = request.Mileage,
                        ImageUrl = request.ImageUrl
                    },
                    Seller = "test" // todo: add current user as seller
                };

                var entityEntry = await dbContext.Auctions.AddAsync(auction);

                if (entityEntry.State != EntityState.Added)
                    return TypedResults.Created("api/auctions", auctionsDto);

                var result = await dbContext.SaveChangesAsync() > 0;

                if (!result) return TypedResults.BadRequest();

                auctionsDto.Id = auction.Id;
                auctionsDto.ReservePrice = auction.ReservePrice;
                auctionsDto.Seller = auction.Seller;
                auctionsDto.Winner = auction.Winner;
                auctionsDto.SoldAmount = auction.SoldAmount;
                auctionsDto.CurrentHighBid = auction.CurrentHighBid;
                auctionsDto.CreatedAt = auction.CreatedAt;
                auctionsDto.UpdatedAt = auction.UpdatedAt;
                auctionsDto.AuctionEnd = auction.AuctionEnd;
                auctionsDto.Status = auction.Status.ToString();
                auctionsDto.Make = auction.Item.Make;
                auctionsDto.Model = auction.Item.Model;
                auctionsDto.Year = auction.Item.Year;
                auctionsDto.Color = auction.Item.Color;
                auctionsDto.Mileage = auction.Item.Mileage;
                auctionsDto.ImageUrl = auction.Item.ImageUrl;

                return TypedResults.Created("api/auctions", auctionsDto);
            });
    }
}