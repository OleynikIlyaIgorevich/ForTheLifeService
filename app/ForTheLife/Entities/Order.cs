using System;
using System.Collections.Generic;

namespace ForTheLife.Entities;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int OrdersStatusId { get; set; }

    public virtual ICollection<OrdersProduct> OrdersProducts { get; set; } = new List<OrdersProduct>();

    public virtual OrdersStatus OrdersStatus { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
