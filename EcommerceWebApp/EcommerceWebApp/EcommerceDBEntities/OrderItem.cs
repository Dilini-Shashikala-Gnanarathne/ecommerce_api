using System;
using System.Collections.Generic;

namespace EcommerceWebApp.EcommerceDBEntities;

public partial class OrderItem
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public long? OrderId { get; set; }

    public virtual Order? Order { get; set; }
}
