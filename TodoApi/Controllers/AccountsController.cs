

using Ganss.Xss;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.ViewModels;

namespace api001.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController(SignInManager<User> signInManager) : ControllerBase
    {
        private readonly HtmlSanitizer _htmlSanitizer = new();

        [HttpGet("ListAllUsers")]
        [EnableRateLimiting("fixed")]
        public async Task<ActionResult<IEnumerable<User>>> ListAllUsers()
        {
            var users = await signInManager.UserManager.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost("register")]
        [EnableRateLimiting("fixed")]
        public async Task<ActionResult> RegisterUser(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid) return ValidationProblem();

            model.Email = _htmlSanitizer.Sanitize(model.Email);
            model.FirstName = _htmlSanitizer.Sanitize(model.FirstName);
            model.LastName = _htmlSanitizer.Sanitize(model.LastName);
            model.Password = _htmlSanitizer.Sanitize(model.Password);

            ModelState.Clear();
            TryValidateModel(model);

            if (!ModelState.IsValid) return ValidationProblem();

            var user = new User
            {
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email
            };

            var result = await signInManager.UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded) return Ok();

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem();
        }

        [HttpPost("logout")]
        [EnableRateLimiting("fixed")]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return NoContent();
        }


        [HttpPost("login")]
        [EnableRateLimiting("fixed")]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return ValidationProblem();

            model.Email = _htmlSanitizer.Sanitize(model.Email);
            model.Password = _htmlSanitizer.Sanitize(model.Password);

            ModelState.Clear();
            TryValidateModel(model);

            if (!ModelState.IsValid) return ValidationProblem();

            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);

            if (result.Succeeded) return Ok();

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return ValidationProblem();
        }

        [HttpPost("registerUser")]
        [EnableRateLimiting("fixed")]
        public async Task<ActionResult> RegisterUser(PostUserViewModel model)
        {
            if (!ModelState.IsValid) return ValidationProblem();

            model.FirstName = _htmlSanitizer.Sanitize(model.FirstName);
            model.LastName = _htmlSanitizer.Sanitize(model.LastName);

            ModelState.Clear();
            TryValidateModel(model);

            if (!ModelState.IsValid) return ValidationProblem();

            var user = new User
            {
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email
            };

            var result = await signInManager.UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded) return Ok();

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem();
        }
    }
}
