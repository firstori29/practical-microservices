namespace SearchService.Endpoints;

internal interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}