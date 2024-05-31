using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Models.DTOs;

namespace TodoApi.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager) : base(userManager, signInManager, roleManager) { }

        [HttpPost, Route("login")]
        public async Task<ActionResult> Login(LoginDTO login)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new { model = login,
                    errorMessage = "Invalid username or password",
                    details = ModelState
                });
            }
            AppUser appUser = await _userManager.FindByEmailAsync(login.Email);
            if(appUser == null)
            {
                return BadRequest(new
                {
                    model = login,
                    errorMessage = "Invalid username or password",
                    details = ModelState
                });
            }

            var identityResult = await
                _signInManager.PasswordSignInAsync(appUser, login.Password,
                isPersistent: false, lockoutOnFailure: false);

            if(identityResult.Succeeded)
            {
                return Ok(new
                {
                    model = login,
                    message = "Login was success!",
                    results = identityResult
                });
            }
            return BadRequest(new
            {
                model = login,
                errorMessage = "Invalid username or password",
                details = ModelState
            });
        }
        [HttpPost, Route("register")]
        public async Task<ActionResult> Register(RegisterDTO register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    model = register,
                    errorMessage = "Failed to create user",
                    details = ModelState
                });
            }
            AppUser appuser = new AppUser
            {
                UserName = register.UserName,
                Email = register.Email,
                PhoneNumber = register.PhoneNumber,

            };

            var results = await _userManager.CreateAsync(appuser, register.Password);
            if (results.Succeeded)
            {
                var role = _roleManager.FindByNameAsync("AppUser").Result;
                if(role == null)
                {
                    role = new IdentityRole
                    {
                        Name = "AppUser"
                    };
                    await _roleManager.CreateAsync(role);
                }
                await _userManager.AddToRoleAsync(appuser, role.Name);

                return CreatedAtAction(nameof(Register), new
                {
                    model = register,
                    message = "user created successful",
                    details = results
                });
            }
            return BadRequest(new
            {
                model = register,
                errorMessage = "Failed to create user",
                details = ModelState
            });
        }
        [HttpPost, Route("logout")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("user signed out");
        }
        [HttpGet, Route("AccessDenied")]
        public  ActionResult AccessDenied()
        {
            return StatusCode(400, new { Message = "Access Denied", Identity = User.Identity });
        }
    }
}
