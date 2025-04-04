using System;
using System.Collections.Generic;

namespace EcommerceWebApp.EcommerceDBEntities;

public partial class Cart
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? UserNic { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
