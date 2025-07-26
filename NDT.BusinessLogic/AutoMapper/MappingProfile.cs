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
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
            

            //CreateMap<NewsRequestDTO, News>()
            //    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
            CreateMap<NewsRequestDTO, News>()
                .ForMember(dest => dest.AuthorId, opt =>
                {
                    opt.Condition(src => src.AuthorId.HasValue);
                    opt.MapFrom(src => src.AuthorId.Value);
                })
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // Category mappings
            CreateMap<Category, CategoryResponseDTO>();
            CreateMap<CategoryRequestDTO, Category>();

            // Tag mappings
            CreateMap<Tags, AdminTagResponseDTO>().ForMember(dest => dest.NewsCount, opt => opt.MapFrom(src => src.News.Count));
            CreateMap<Tags, TagResponseDTO>().ForMember(dest => dest.NewsCount, opt => opt.MapFrom(src => src.News.Count));

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
