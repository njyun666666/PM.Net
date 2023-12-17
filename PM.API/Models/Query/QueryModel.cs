using System.Text.Json.Serialization;

namespace PMAPI.Models.Query
{
	public class QueryModel<T>
	{
		public T Filter { get; set; }
		public int PageIndex { get; set; }
		public int PageSize { get; set; }
		public string? Sort { get; set; }
		public bool Desc { get; set; }
		public string OrderBy => Sort + (Desc ? " DESC" : "");
		public int Skip => PageIndex * PageSize;
	}

	public class QueryViewModel<T>
	{
		public T Data { get; set; }
		[JsonIgnore]
		public int Count { get; set; }
		[JsonIgnore]
		public int PageSize { get; set; }
		public int PageCount => (int)Math.Ceiling((decimal)Count / PageSize);
	}
}
