using System;
using System.Collections.Generic;

namespace PMDB.Models;

public partial class VwOrgUser
{
    public string Uid { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Password { get; set; }

    public string Name { get; set; } = null!;

    public string? PhotoUrl { get; set; }

    public bool Enable { get; set; }

    public string? OauthProvider { get; set; }

    public string Did { get; set; } = null!;

    public string DeptName { get; set; } = null!;

    public string RootDid { get; set; } = null!;

    public string? CompanyName { get; set; }
}
