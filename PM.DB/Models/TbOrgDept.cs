using System;
using System.Collections.Generic;

namespace PMDB.Models;

public partial class TbOrgDept
{
    public string Did { get; set; } = null!;

    public string? DeptName { get; set; }

    public string? ParentDid { get; set; }

    public string? RootDid { get; set; }

    public bool? Enable { get; set; }

    public int? Sort { get; set; }

    public bool? Expand { get; set; }

    public virtual ICollection<TbOrgDeptUser> TbOrgDeptUsers { get; set; } = new List<TbOrgDeptUser>();
}
