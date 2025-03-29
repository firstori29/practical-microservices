namespace SearchService.Services;

internal sealed class AuctionServiceHttpClient(HttpClient httpClient, IConfiguration config)
{
    public async Task<List<Item>> GetItemsForSearchDbAsync()
    {
        var lastUpdated = await DB.Find<Item, string>()
            .Sort(item => item.Descending(x => x.UpdatedAt))
            .Project(x => x.UpdatedAt.ToString())
            .ExecuteFirstAsync();

        var auctionUrl = config["AuctionServiceUrl"];

        return await httpClient.GetFromJsonAsync<List<Item>>(
            $"{auctionUrl}/api/auctions?date={lastUpdated}");
    }
}