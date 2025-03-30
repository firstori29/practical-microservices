namespace SearchService.Consumers;

public sealed class AuctionCreatedConsumer(IMapper mapper) : IConsumer<AuctionCreated>
{
    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        Console.WriteLine($"--> Consuming auction created: {context.Message.Id}");

        var item = mapper.Map<Item>(context.Message);

        await item.SaveAsync();
    }
};