using System;
using System.Collections.Generic;

namespace PMDB.Models;

public partial class VwOrgCompanyUser
{
    public string Did { get; set; } = null!;

    public string Uid { get; set; } = null!;

    public bool Enable { get; set; }

    public string DeptName { get; set; } = null!;
}
