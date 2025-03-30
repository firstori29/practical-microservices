namespace Contracts;

public sealed record AuctionCreated(
    Guid Id,
    int ReservePrice,
    string Seller,
    string Winner,
    int? SoldAmount,
    int? CurrentHighBid,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime AuctionEnd,
    string Status,
    string Make,
    string Model,
    int Year,
    string Color,
    int Mileage,
    string ImageUrl);