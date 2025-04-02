namespace SearchService.Consumers;

public sealed class AuctionCreatedConsumer(IMapper mapper) : IConsumer<AuctionCreated>
{
    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        Console.WriteLine($"--> Consuming auction created: {context.Message.Id}");

        var item = mapper.Map<Item>(context.Message);

        if (item.Model == "Foo") throw new ArgumentException("Cannot sell cars with name of Foo");

        await item.SaveAsync();
    }
};