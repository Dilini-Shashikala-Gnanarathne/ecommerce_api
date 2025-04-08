using EcommerceWebApp.BaseDBEntities;
using EcommerceWebApp.EcommerceDBEntities;

namespace EcommerceWebApp.Models
{
    public class OrderItemData
    {
        public Order Order { get; set; }
        public OrderItem OrderItem { get; set; }
        public Product Product { get; set; }
        public User User { get; set; }
        public Cart Cart { get; set; }
    }
}
