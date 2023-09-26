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
		public string _uid
		{
			get
			{
				return GetUserClaim("uid");
			}
		}

		[NonAction]
		public string GetUserClaim(string ClaimName)
		{
			string value = string.Empty;
			var user = HttpContext.User;

			if (user.HasClaim(c => c.Type == ClaimName))
			{
				string? v = user.Claims.FirstOrDefault(c => c.Type == ClaimName)?.Value;
				if (v != null)
				{
					value = v;
				}
			}
			return value;
		}
	}
}
