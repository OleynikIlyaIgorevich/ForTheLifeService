using System;
using System.Collections.Generic;

namespace ForTheLife.Entities;

public partial class OrdersStatus
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
