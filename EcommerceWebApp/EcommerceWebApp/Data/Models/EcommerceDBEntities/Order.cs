using System;
using System.Collections.Generic;

namespace EcommerceWebApp.EcommerceDBEntities;

public partial class Order
{
    public long Id { get; set; }

    public string OrderId { get; set; } = null!;

    public string OrderName { get; set; } = null!;

    public decimal Price { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? UserNic { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
