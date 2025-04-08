namespace EcommerceWebApp.Models
{
    public class ProductDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; } // Assuming category is a string here
    }

}
