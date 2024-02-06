using PMAPI.Models.Query;

namespace PMAPI.Helper
{
	public static class QueryExtensions
	{
		public static int PageCount<T>(this QueryModel<T> model, int count)
		{
			return (int)Math.Ceiling((decimal)count / model.PageSize);
		}
	}
}
