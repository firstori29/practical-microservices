namespace SearchService.Endpoints;

internal sealed class SearchItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/search", async Task<IResult> (string searchTerm, 
            int pageNumber = 1, int pageSize = 4) =>
        {
            var query = DB.PagedSearch<Item>();

            query.Sort(builder => builder.Ascending(i => i.Make));

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query.Match(Search.Full, searchTerm).SortByTextScore();
            }

            query.PageNumber(pageNumber);
            query.PageSize(pageSize);

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