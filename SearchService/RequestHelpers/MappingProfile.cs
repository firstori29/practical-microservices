namespace SearchService.RequestHelpers;

internal sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AuctionCreated, Item>();
    }
}