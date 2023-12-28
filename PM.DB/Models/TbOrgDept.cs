using System;
using System.Collections.Generic;

namespace PMDB.Models;

public partial class TbOrgDept
{
    public string Did { get; set; } = null!;

    public string DeptName { get; set; } = null!;

    public string? ParentDid { get; set; }

    public string RootDid { get; set; } = null!;

    public bool Enable { get; set; }

    public int Sort { get; set; }

    public bool Expand { get; set; }

    public string? LogId { get; set; }

    public virtual ICollection<TbOrgDeptUser> TbOrgDeptUsers { get; set; } = new List<TbOrgDeptUser>();

    public virtual ICollection<TbOrgRoleUser> TbOrgRoleUsers { get; set; } = new List<TbOrgRoleUser>();
}
