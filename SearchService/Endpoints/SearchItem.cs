namespace SearchService.Endpoints;

internal sealed class SearchItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/search", async Task<IResult> ([AsParameters] SearchParams searchParams) =>
        {
            var query = DB.PagedSearch<Item, Item>();

            if (!string.IsNullOrEmpty(searchParams.SearchTerm))
            {
                query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
            }

            query = searchParams.OrderBy switch
            {
                "make" => query.Sort(builder => builder.Ascending(item => item.Make)),
                "new" => query.Sort(builder => builder.Ascending(item => item.CreatedAt)),
                _ => query.Sort(builder => builder.Ascending(item => item.AuctionEnd))
            };

            query = searchParams.FilterBy switch
            {
                "finished" => query.Match(builder => builder.AuctionEnd < DateTime.UtcNow),
                "endingSoon" => query.Match(builder => builder.AuctionEnd < DateTime.UtcNow.AddHours(6)
                                                       && builder.AuctionEnd > DateTime.UtcNow),
                _ => query.Match(builder => builder.AuctionEnd > DateTime.UtcNow)
            };

            if (!string.IsNullOrEmpty(searchParams.Seller))
            {
                query.Match(builder => builder.Seller == searchParams.Seller);
            }

            if (!string.IsNullOrEmpty(searchParams.Winner))
            {
                query.Match(builder => builder.Winner == searchParams.Winner);
            }

            query.PageNumber(searchParams.PageNumber);
            query.PageSize(searchParams.PageSize);

            var result = await query.ExecuteAsync();

            return TypedResults.Ok(new
            {
                results = result.Results,
                pageCount = result.PageCount,
                totalCount = result.TotalCount,
            });
        });
    }
}