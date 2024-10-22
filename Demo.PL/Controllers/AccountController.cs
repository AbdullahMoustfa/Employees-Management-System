using Demo.DAL.Data.Migrations;
using Demo.DAL.Models;
using Demo.PL.Helpers.Services.EmailSender;
using Demo.PL.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
//using Microsoft.VisualStudio.Web.CodeGeneration;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{

    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            IEmailSender emailSender,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _configuration = configuration;
            _emailSender = emailSender;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        #region Sign Up

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user is null)
                {
                    user = new ApplicationUser()
                    {
                        FName = model.FirstName,
                        LName = model.LastName,
                        UserName = model.Username,
                        Email = model.Email,
                        IsAgree = model.IsAgree,
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                        return RedirectToAction(nameof(SignIn));

                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }

                ModelState.AddModelError(string.Empty, "this username is already in use for another account!");
            }
            return View(model);
        }
        #endregion

        #region Sign In

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);

                    if (flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                        if (result.IsLockedOut)
                            ModelState.AddModelError(string.Empty, "Your Account isn't Locked");

                        if (result.Succeeded)
                            return RedirectToAction(nameof(HomeController.Index), "Home");

                        if (result.IsNotAllowed)
                            ModelState.AddModelError(string.Empty, "Your Account isn't Confirmed yet!!");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Login");
            }
            return View(model);
        }

        #endregion

        #region Sign Out
        public async new Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }

        #endregion

        #region Forget Password
        public IActionResult ForgetPassword()
        {
            return View();
        }

        public async Task<IActionResult> SendResetPasswordEmail(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);  // UNIQUE

                    var resetPasswordUrl = Url.Action("ResetPassword", "Account", new { email = user.Email });

                    // https://localhost:5001/Account/ResetPassword?email=abdullah@gmail.com&token=1fsdfjief4fjdjfid4

                    await _emailSender.SendAsync(
                        from: _configuration["EmailSettings:SenderEmail"],
                        recipients: model.Email,
                        subject: "Reset Your Password",
                        body: resetPasswordUrl);
                    return RedirectToAction(nameof(CheckYourInbox));


                }
                ModelState.AddModelError(string.Empty, "There is No Account with this Email!!");
            }
            return View(model);
        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }
        #endregion

        #region Reset Password 

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["Email"] = email;
            TempData["Token"] = token;  
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
           
            if (ModelState.IsValid) 
            {
                var email = TempData["Email"] as string;
                var token = TempData["Token"] as string;

                var user  = await _userManager.FindByEmailAsync(email); 


                if (user is not null)
                {
					await _userManager.ResetPasswordAsync(user,token,model.NewPassword);
                    return RedirectToAction(nameof(SignIn));
				}
                ModelState.AddModelError(string.Empty, "Url is not valid");
			} 
            return View(model);
        }
        #endregion


    }
}
