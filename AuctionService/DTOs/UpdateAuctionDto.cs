namespace AuctionService.DTOs;

internal sealed class UpdateAuctionDto
{
    public string Make { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public int Year { get; set; }

    public string Color { get; set; } = string.Empty;

    public int Mileage { get; set; }
}