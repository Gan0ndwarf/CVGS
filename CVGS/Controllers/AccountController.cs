using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using CVGS.Models;
using CVGS.Models.AccountViewModels;
using CVGS.Services;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http;


namespace CVGS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly CVGSContext _context;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            CVGSContext context,
            IEmailSender emailSender,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailSender = emailSender;
            _logger = logger;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: true);


                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["GenderList"] = new SelectList(_context.LookupGender.OrderBy(p => p.Id), "Id", "Gender");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                int? userId = AddUserToDataBase(model);

                if(userId != null)
                {

                    // Set session variables for newly registered user
                    IQueryable<User> myUsers = from u in _context.User
                                               where u.UserId == userId
                                               select u;

                    User registeredUser = myUsers.FirstOrDefault();

                    if (registeredUser.EmployeeFlag)
                    {
                        HttpContext.Session.SetString("isEmp", true.ToString());                
                    }
                    else
                    {
                        HttpContext.Session.SetString("isEmp", false.ToString());
                    }
                    HttpContext.Session.SetString("userId", registeredUser.UserId.ToString());


                    List<Game> games = new List<Game>();
                    HttpContext.Session.SetObjectAsJson("cart", games);
                    HttpContext.Session.SetString("cartCount", games.Count().ToString());

                    HttpContext.Session.SetString("subTotal", "0.00");
                    HttpContext.Session.SetString("tax", "0.00");
                    HttpContext.Session.SetString("total", "0.00");


                    // Set app user
                    var user = new ApplicationUser { Id = userId.ToString(), UserName = model.Username, PasswordHash = model.Password, Email = model.Email };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                        //await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created a new account with password.");
                        return RedirectToLocal(returnUrl);
                    }
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["GenderList"] = new SelectList(_context.LookupGender.OrderBy(p => p.Id), "Id", "Gender");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action(
            action: nameof(AccountController.ResetPassword),
            controller: "Account",
            values: new { user.Id, code },
            protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action(
            action: nameof(AccountController.ResetPassword),
            controller: "Account",
            values: new { user.Id, code },
            protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ChangePasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                ChangePassword(Convert.ToInt32(user.Id), model.Password);
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion

        private int? AddUserToDataBase(RegisterViewModel model)
        {
            SqlConnection SqlConn = new SqlConnection("Data Source=.;Initial Catalog=CVGS;Integrated Security=True");
            SqlCommand sqlcomm = new SqlCommand("uspAddUser", SqlConn);

            try
            {
                SqlConn.Open();
                sqlcomm.CommandType = CommandType.StoredProcedure;

                SqlParameter empFlagParam = new SqlParameter("@pEmployeeFlag", SqlDbType.Bit);
                empFlagParam.Direction = ParameterDirection.Input;
                empFlagParam.Value = 0;
                sqlcomm.Parameters.Add(empFlagParam);

                SqlParameter privFlagParam = new SqlParameter("@pPrivateFlag", SqlDbType.Bit);
                privFlagParam.Direction = ParameterDirection.Input;
                privFlagParam.Value = model.PrivateFlag;
                sqlcomm.Parameters.Add(privFlagParam);

                SqlParameter fNameParam = new SqlParameter("@pFirstName", SqlDbType.NVarChar);
                fNameParam.Direction = ParameterDirection.Input;
                fNameParam.Size = 50;
                fNameParam.Value = model.FirstName;
                sqlcomm.Parameters.Add(fNameParam);

                SqlParameter lNameParam = new SqlParameter("@pLastName", SqlDbType.NVarChar);
                lNameParam.Direction = ParameterDirection.Input;
                lNameParam.Size = 50;
                lNameParam.Value = model.LastName;
                sqlcomm.Parameters.Add(lNameParam);

                SqlParameter userNameParam = new SqlParameter("@pUsername", SqlDbType.NVarChar);
                userNameParam.Direction = ParameterDirection.Input;
                userNameParam.Size = 50;
                userNameParam.Value = model.Username;
                sqlcomm.Parameters.Add(userNameParam);

                SqlParameter passwordParam = new SqlParameter("@pPassword", SqlDbType.NVarChar);
                passwordParam.Direction = ParameterDirection.Input;
                passwordParam.Size = 50;
                passwordParam.Value = model.Password;
                sqlcomm.Parameters.Add(passwordParam);

                SqlParameter emailParam = new SqlParameter("@pEmail", SqlDbType.NVarChar);
                emailParam.Direction = ParameterDirection.Input;
                emailParam.Size = 50;
                emailParam.Value = model.Email;
                sqlcomm.Parameters.Add(emailParam);

                SqlParameter genderIdParam = new SqlParameter("@pGenderId", SqlDbType.Int);
                genderIdParam.Direction = ParameterDirection.Input;
                genderIdParam.Value = model.GenderId;
                sqlcomm.Parameters.Add(genderIdParam);

                SqlParameter dobParam = new SqlParameter("@pDOB", SqlDbType.Date);
                dobParam.Direction = ParameterDirection.Input;
                dobParam.Value = model.BirthDate;
                sqlcomm.Parameters.Add(dobParam);

                SqlParameter emailFlagParam = new SqlParameter("@pEmailFlag", SqlDbType.Bit);
                emailFlagParam.Direction = ParameterDirection.Input;
                emailFlagParam.Value = model.EmailFlag;
                sqlcomm.Parameters.Add(emailFlagParam);

                SqlParameter phoneParam = new SqlParameter("@pPhone", SqlDbType.NChar);
                phoneParam.Direction = ParameterDirection.Input;
                phoneParam.Size = 10;
                phoneParam.Value = model.Phone;
                sqlcomm.Parameters.Add(phoneParam);

                SqlParameter messageParam = new SqlParameter("@responseMessage", SqlDbType.NVarChar);
                messageParam.Direction = ParameterDirection.Output;
                messageParam.Size = 250;
                messageParam.Value = null;
                sqlcomm.Parameters.Add(messageParam);

                SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int);
                idParam.Direction = ParameterDirection.Output;
                idParam.Size = 250;
                idParam.Value = null;
                sqlcomm.Parameters.Add(idParam);

                sqlcomm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            int? userId = Convert.ToInt32(sqlcomm.Parameters["@id"].Value);

            SqlConn.Close();

            return userId;
        }


        private void ChangePassword(int userId, string password)
        {
            SqlConnection SqlConn = new SqlConnection("Data Source=.;Initial Catalog=CVGS;Integrated Security=True");
            SqlCommand sqlcomm = new SqlCommand("uspChangePassword", SqlConn);

            try
            {
                SqlConn.Open();
                sqlcomm.CommandType = CommandType.StoredProcedure;

                SqlParameter userIdParam = new SqlParameter("@pUserID", SqlDbType.Int);
                userIdParam.Direction = ParameterDirection.Input;
                userIdParam.Value = userId;
                sqlcomm.Parameters.Add(userIdParam);

                SqlParameter newPasswordParam = new SqlParameter("@pNewPassword", SqlDbType.NVarChar);
                newPasswordParam.Direction = ParameterDirection.Input;
                newPasswordParam.Size = 50;
                newPasswordParam.Value = password;
                sqlcomm.Parameters.Add(newPasswordParam);

                sqlcomm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            SqlConn.Close();

        }
    }
}
