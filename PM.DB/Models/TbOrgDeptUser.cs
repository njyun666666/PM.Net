using System;
using System.Collections.Generic;

namespace PMDB.Models;

public partial class TbOrgDeptUser
{
    public string Did { get; set; } = null!;

    public string Uid { get; set; } = null!;

    public bool? Enable { get; set; }

    public virtual TbOrgDept DidNavigation { get; set; } = null!;

    public virtual TbOrgUser UidNavigation { get; set; } = null!;
}
