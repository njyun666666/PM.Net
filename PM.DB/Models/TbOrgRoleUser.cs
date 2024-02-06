using System;
using System.Collections.Generic;

namespace PMDB.Models;

public partial class TbOrgRoleUser
{
    public string Uid { get; set; } = null!;

    public string Rid { get; set; } = null!;

    public string RootDid { get; set; } = null!;

    public virtual TbOrgRole RidNavigation { get; set; } = null!;

    public virtual TbOrgDept RootD { get; set; } = null!;

    public virtual TbOrgUser UidNavigation { get; set; } = null!;
}
