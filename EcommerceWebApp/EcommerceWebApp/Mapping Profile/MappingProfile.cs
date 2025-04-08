using AutoMapper;
using EcommerceWebApp.EcommerceDBEntities;
using Newtonsoft.Json;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Define existing mappings
        CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Images)
                    ? new List<string>()
                    : JsonConvert.DeserializeObject<List<string>>(src.Images)
            ));

        CreateMap<CartItem, CartItemResponse>(); // required!
        CreateMap<Cart, CartResponse>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.CartItems));

        // Add mapping for Order -> OrderResponse
        CreateMap<Order, OrderResponse>();
    }
}
