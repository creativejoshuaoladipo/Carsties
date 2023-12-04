using AuctionServices.Controllers;
using AuctionsServices.Entites;
using AuctionsServices.Entites.Dtos;
using AutoMapper;

namespace AuctionsServices.Helpers;
public class CarstiesProfile : Profile
{

    public CarstiesProfile()
    {
        CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
        CreateMap<Item, AuctionDto>();

        CreateMap<CreateAuctionDto, Auction>()
    .ForMember(dest => dest.Item, opt => opt.MapFrom(src => new Item
    {
        Make = src.Make,
        Model = src.Model,
        Year = src.Year,
        Mileage = src.Mileage,
        Color = src.Color,
        ImageUrl = src.ImageUrl
    }))
    .ForMember(dest => dest.ReservePrice, opt => opt.MapFrom(src => src.ReservePrice))
    .ForMember(dest => dest.AuctionEnd, opt => opt.MapFrom(src => src.AuctionEnd));
        // Map other fields as necessary

    }
}
