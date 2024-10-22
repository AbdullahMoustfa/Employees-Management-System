using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels.Account
{
    public class ForgetPasswordViewModel
    {
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }
	}
}
