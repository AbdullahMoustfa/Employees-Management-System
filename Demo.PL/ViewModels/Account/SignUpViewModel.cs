using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels.Account
{
	public class SignUpViewModel
	{
        [Required(ErrorMessage ="User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage ="Email is required")]
		[EmailAddress(ErrorMessage ="Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage ="First Name is required")]
        [Display(Name = "First Name")]
        public string FirstName {  get; set; }

		[Required(ErrorMessage = "Last Name is required")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		[Required(ErrorMessage ="Password is required")]
       //  [MinLength(5,ErrorMessage ="Minimum Password Length is 5")]
        [DataType(DataType.Password)]   
        public string Password { get; set; }

        [Required(ErrorMessage ="Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Confirm Password doesn't match with this Password")]
        public string ConfirmPassword { get; set; }

        public bool IsAgree { get; set; }
    }
}
