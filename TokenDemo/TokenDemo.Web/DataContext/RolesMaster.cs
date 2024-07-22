using System;
using System.Collections.Generic;

namespace TokenDemo.Web.DataContext;

public partial class RolesMaster
{
    public long RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
