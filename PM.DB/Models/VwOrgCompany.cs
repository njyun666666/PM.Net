using System;
using System.Collections.Generic;

namespace PMDB.Models;

public partial class VwOrgCompany
{
    public string Did { get; set; } = null!;

    public string DeptName { get; set; } = null!;

    public bool Enable { get; set; }

    public int Sort { get; set; }

    public bool Expand { get; set; }
}
