﻿namespace SearchService.Data;

internal static class DbInitializer
{
    public static async Task InitDb(WebApplication app)
    {
        await DB.InitAsync("SearchDb",
            MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

        await DB.Index<Item>()
            .Key(i => i.Make, KeyType.Text)
            .Key(i => i.Model, KeyType.Text)
            .Key(i => i.Color, KeyType.Text)
            .CreateAsync();

        // var count = await DB.CountAsync<Item>();

        // if (count == 0)
        // {
        //     Console.WriteLine("No data - will attempt to seed");
        //
        //     var itemData = await File.ReadAllTextAsync("Data/auctions.json");
        //
        //     var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        //
        //     var items = JsonSerializer.Deserialize<List<Item>>(itemData, options);
        //
        //     await DB.SaveAsync(items!);
        // }
        
        using var scope = app.Services.CreateScope();
        
        var httpClient = scope.ServiceProvider.GetRequiredService<AuctionServiceHttpClient>();

        var items = await httpClient.GetItemsForSearchDbAsync();

        Console.WriteLine($"Found {items.Count} items received from auction service.");
        
        if (items.Count > 0) await DB.SaveAsync(items);
    }
}