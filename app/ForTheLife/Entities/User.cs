using System;
using System.Collections.Generic;

namespace ForTheLife.Entities;

public partial class User
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public string Lastname { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string? Middlename { get; set; }

    public string Username { get; set; } = null!;

    public string Pass { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role Role { get; set; } = null!;
}
