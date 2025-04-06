namespace SearchService.Consumers;

public sealed class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
{
    public async Task Consume(ConsumeContext<AuctionDeleted> context)
    {
        var id = context.Message.Id;

        Console.WriteLine($"--> Consuming auction deleted: {id}");

        await DB.DeleteAsync<Item>(id);
    }
}