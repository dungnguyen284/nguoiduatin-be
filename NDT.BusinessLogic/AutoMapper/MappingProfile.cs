using AutoMapper;
using NDT.BusinessLogic.DTOs.RequestDTOs;
using NDT.BusinessLogic.DTOs.ResponseDTOs;
using NDT.BusinessModels.Entities;

namespace NDT.BusinessLogic.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // News mappings
            CreateMap<News, NewsResponseDTO>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.FullName))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags));

            CreateMap<NewsRequestDTO, News>();

            // Category mappings
            CreateMap<Category, CategoryResponseDTO>();
            CreateMap<CategoryRequestDTO, Category>();

            // Tag mappings
            CreateMap<Tags, TagResponseDTO>();
            CreateMap<TagRequestDTO, Tags>();

            // Advertisement mappings
            CreateMap<Advertisement, AdvertisementResponseDTO>();
            CreateMap<AdvertisementRequestDTO, Advertisement>();

            // User mappings
            CreateMap<AppUser, UserResponseDTO>();
            CreateMap<UserRequestDTO, AppUser>();

            // Stock mappings
            CreateMap<Stock, StockResponseDTO>();
            CreateMap<StockRequestDTO, Stock>();
        }
    }
}
