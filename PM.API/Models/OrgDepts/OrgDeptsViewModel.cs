﻿namespace PMAPI.Models.OrgDepts
{
	public class OrgDeptsViewModel
	{
		public string Did { get; set; } = null!;

		public string DeptName { get; set; } = null!;

		public string? ParentDid { get; set; }
		public string? ParentDeptName { get; set; }

		public string RootDid { get; set; } = null!;

		public string? CompanyName { get; set; }

		public bool Enable { get; set; }

		public int Sort { get; set; }

		public bool Expand { get; set; }
	}
}
