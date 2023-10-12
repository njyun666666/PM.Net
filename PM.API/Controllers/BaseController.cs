using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PMAPI.Controllers
{
	[Controller]
	public class BaseController : ControllerBase
	{
		public BaseController()
		{

		}
		public string _uid => GetUserClaim("uid").FirstOrDefault();

		[NonAction]
		public List<string> GetUserClaim(string ClaimName)
		{
			var user = HttpContext.User;

			if (user.HasClaim(c => c.Type == ClaimName))
			{
				var v = user.Claims.Where(c => c.Type == ClaimName).Select(x => x.Value).ToList();
				if (v != null)
				{
					return v;
				}
			}

			return new List<string>();
		}
	}
}
