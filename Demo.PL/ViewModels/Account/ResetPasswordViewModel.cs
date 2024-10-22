using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels.Account
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage = "New Password is required")]
		[MinLength(5, ErrorMessage = "Minimum Password Length is 5")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "Confirm Password is required")]
		[DataType(DataType.Password)]
		[Compare(nameof(NewPassword), ErrorMessage = "Confirm Password doesn't match with New Password")]
		public string ConfirmPassword { get; set; }
        public string email { get; set; }
        public string token { get; set; }
    }
}
