﻿using System;
using System.Collections.Generic;

namespace PMDB.Models;

public partial class TbOrgUser
{
    public string Uid { get; set; } = null!;

    public string? Email { get; set; }

    public string? Passwrod { get; set; }

    public string? Name { get; set; }

    public bool? Enable { get; set; }

    public string? OauthProvider { get; set; }

    public virtual ICollection<TbOrgDeptUser> TbOrgDeptUsers { get; set; } = new List<TbOrgDeptUser>();

    public virtual ICollection<TbOrgRole> Rids { get; set; } = new List<TbOrgRole>();
}
