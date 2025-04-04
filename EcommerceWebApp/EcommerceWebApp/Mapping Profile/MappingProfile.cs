using AutoMapper;
using EcommerceWebApp.EcommerceDBEntities;
using Newtonsoft.Json;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Images) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(src.Images)
            ));
    }
}
