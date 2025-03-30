namespace Contracts;

public sealed record AuctionUpdated(
    string Id,
    string? Make,
    string? Model,
    int? Year,
    string? Color,
    int?  Mileage);