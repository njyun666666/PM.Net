namespace PMAPI.Models.OrgUser
{
	public class OrgUserModel
	{
		public string Uid { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Name { get; set; } = null!;
		//public string? PhotoUrl { get; set; } = null;
		public bool Enable { get; set; }
		//public string? LogId { get; set; }
		public List<OrgUserDeptModel> Depts { get; set; } = new List<OrgUserDeptModel>();
	}
	public class OrgUserDeptModel
	{
		public string Did { get; set; } = null!;
	}
	public class OrgUserQueryModel
	{
		public string RootDID { get; set; } = null!;
		public string? Name { get; set; } = null;
		public string? EMail { get; set; } = null;
	}
	public class OrgUserViewModel
	{
		public string Uid { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Name { get; set; } = null!;
		public string? PhotoUrl { get; set; } = null;
		public bool Enable { get; set; }
		//public string? LogId { get; set; }
	}
}
