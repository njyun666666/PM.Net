using System.Text.Json.Serialization;

namespace PMAPI.Models.Menu
{
	public class MenuViewModel
	{
		public string MenuId { get; set; } = null!;
		[JsonIgnore]
		public string? ParentMenuId { get; set; }
		public string MenuName { get; set; } = null!;
		public string? Icon { get; set; }
		public string? Url { get; set; }
		[JsonIgnore]
		public bool Enable { get; set; }
		[JsonIgnore]
		public int Sort { get; set; }
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<MenuViewModel>? Children { get; set; } = null;
	}

	public class AuthMenuViewModel
	{
		public string MenuId { get; set; } = null!;
		public string? ParentMenuId { get; set; }
		public string MenuName { get; set; } = null!;
		public string? Icon { get; set; }
		public string? Url { get; set; }
		public bool Enable { get; set; }
		public int Sort { get; set; }
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<AuthMenuViewModel>? Children { get; set; } = null;
	}
}
