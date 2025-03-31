namespace AuctionService.DTOs;

internal sealed record UpdateAuctionDto
{
    public string? Make { get; init; } = string.Empty;

    public string? Model { get; set; } = string.Empty;

    public int? Year { get; set; }

    public string? Color { get; set; } = string.Empty;

    public int? Mileage { get; set; }
}