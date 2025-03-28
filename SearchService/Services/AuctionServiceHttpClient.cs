namespace SearchService.Services;

internal sealed class AuctionServiceHttpClient(HttpClient httpClient, IConfiguration config)
{
    public async Task<List<Item>> GetItemsForSearchDbAsync()
    {
        var lastUpdated = await DB.Find<Item, string>()
            .Sort(item => item.Descending(x => x.UpdatedAt))
            .Project(x => x.UpdatedAt.ToString())
            .ExecuteFirstAsync();

        return await httpClient.GetFromJsonAsync<List<Item>>(
            $"{config["AuctionServiceUrl"]}/api/auctions?date={lastUpdated}");
    }
}