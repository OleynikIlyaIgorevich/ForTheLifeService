using System;
using System.Collections.Generic;

namespace ForTheLife.Entities;

public partial class Supplier
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
