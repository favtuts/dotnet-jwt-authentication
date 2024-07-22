using System;
using System.Collections.Generic;

namespace TokenDemo.Web.DataContext;

public partial class RefreshToken
{
    public long RefreshTokenId { get; set; }

    public string Token { get; set; } = null!;

    public string JwtId { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    public bool? Used { get; set; }

    public long UserId { get; set; }

    public virtual UsersMaster User { get; set; } = null!;
}
