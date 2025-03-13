using System;
using System.Collections.Generic;

namespace ForTheLife.Entities;

public partial class Product
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public int SupplierId { get; set; }

    public int ProducerId { get; set; }

    public int ProductNameId { get; set; }

    public int UnitId { get; set; }

    public string Article { get; set; } = null!;

    public decimal Price { get; set; }

    public int CurrentSale { get; set; }

    public int MaxSale { get; set; }

    public int Count { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrdersProduct> OrdersProducts { get; set; } = new List<OrdersProduct>();

    public virtual Producer Producer { get; set; } = null!;

    public virtual ProductName ProductName { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual Unit Unit { get; set; } = null!;
}
