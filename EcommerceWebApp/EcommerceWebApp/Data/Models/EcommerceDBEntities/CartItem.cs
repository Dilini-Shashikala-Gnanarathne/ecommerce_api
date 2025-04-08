using System;
using System.Collections.Generic;

namespace EcommerceWebApp.EcommerceDBEntities;

public partial class CartItem
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public long? CartId { get; set; }

    public virtual Cart? Cart { get; set; }
}
