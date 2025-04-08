using System;
using System.Collections.Generic;

namespace EcommerceWebApp.EcommerceDBEntities;

public partial class Product
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public string Category { get; set; } = null!;

    public string Images { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? UserNic { get; set; }
}
