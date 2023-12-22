using System;
using System.Collections.Generic;

namespace PMDB.Models;

public partial class TbLog
{
    public long AutoId { get; set; }

    public string LogId { get; set; } = null!;

    public string Uid { get; set; } = null!;

    public DateTime UpdateTime { get; set; }

    public string UpdateTable { get; set; } = null!;

    public string LogState { get; set; } = null!;

    public string? OriginalValues { get; set; }

    public string? CurrentValues { get; set; }
}
