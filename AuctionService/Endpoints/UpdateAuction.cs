namespace AuctionService.Endpoints;

internal sealed class UpdateAuction : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/auctions/{id:guid}",
            async Task<IResult> (Guid id, UpdateAuctionDto updateAuctionDto, AuctionDbContext dbContext) =>
            {
                var auctionsDto = new AuctionDto();

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

                return TypedResults.Ok(auctionsDto);
            });
    }
}