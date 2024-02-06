using System.ComponentModel.DataAnnotations;

namespace PMAPI.Models.Login
{
	public class LoginModel
	{
		[Required(ErrorMessage = "required")]
		public string Email { get; set; }
		[Required(ErrorMessage = "required")]
		public string Password { get; set; }
	}
}
