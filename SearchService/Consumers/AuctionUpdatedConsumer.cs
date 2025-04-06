namespace SearchService.Consumers;

public sealed class AuctionUpdatedConsumer(IMapper mapper) : IConsumer<AuctionUpdated>
{
    public async Task Consume(ConsumeContext<AuctionUpdated> context)
    {
        var id = context.Message.Id;

        Console.WriteLine($"--> Consuming auction updated: {id}");

        var item = mapper.Map<Item>(context.Message);

        await DB.Update<Item>()
            .Match(i => i.ID == id)
            .Modify(i => i.Make, item.Make)
            .Modify(i => i.Model, item.Model)
            .Modify(i => i.Color, item.Color)
            .Modify(i => i.Mileage, item.Mileage)
            .Modify(i => i.Year, item.Year)
            .ExecuteAsync();
    }
}