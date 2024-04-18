using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Niveau.Areas.Admin.Models.Accounts;

namespace Niveau.Areas.User.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _env = env;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "User", code },
                    protocol: Request.Scheme);

                var templatePath = Path.Combine(_env.WebRootPath, "Emails", "ReserPasswordEmail.cshtml");
                var emailBody = PopulateEmailTemplate(templatePath, new Dictionary<string, string>
                    {
                        {"FirstName", user.FirstName},
                        {"ResetLink", callbackUrl}
                    });

                await _emailSender.SendEmailAsync(Input.Email, "I'm here!", emailBody);

   /*             var emailContent = $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Reset Password",
                    emailContent);
                //$"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");*/

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
        private string PopulateEmailTemplate(string filePath, Dictionary<string, string> values)
        {
            string template = System.IO.File.ReadAllText(filePath);

            foreach (var item in values)
            {
                template = template.Replace($"{{{{" + item.Key + "}}", item.Value);
            }
            return template;
        }
    }
}
