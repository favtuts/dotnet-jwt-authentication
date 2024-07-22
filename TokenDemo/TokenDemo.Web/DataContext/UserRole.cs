using System;
using System.Collections.Generic;

namespace TokenDemo.Web.DataContext;

public partial class UserRole
{
    public long UserRoleId { get; set; }

    public long UserId { get; set; }

    public long RoleId { get; set; }

    public virtual RolesMaster Role { get; set; } = null!;

    public virtual UsersMaster User { get; set; } = null!;
}
