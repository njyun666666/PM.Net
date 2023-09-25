using System;
using System.Collections.Generic;

namespace PMDB.Models;

public partial class TbOrgRole
{
    public string Rid { get; set; } = null!;

    public string? RoleName { get; set; }

    public virtual ICollection<TbMenu> Menus { get; set; } = new List<TbMenu>();

    public virtual ICollection<TbOrgUser> Uids { get; set; } = new List<TbOrgUser>();
}
