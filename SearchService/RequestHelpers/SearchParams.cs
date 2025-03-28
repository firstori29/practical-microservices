namespace SearchService.RequestHelpers;

internal sealed record SearchParams(
    string SearchTerm,
    string Seller,
    string Winner,
    string OrderBy,
    string FilterBy,
    int PageNumber = 1,
    int PageSize = 4);