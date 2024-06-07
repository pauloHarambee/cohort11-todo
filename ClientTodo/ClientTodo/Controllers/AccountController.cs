using ClientTodo.Models;
using ClientTodo.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ClientTodo.Controllers
{
    [Authorize]
    public class AccountController : BaseWebController
    {
        public AccountController(Options options, HttpClient httpClient) : base(options, httpClient)
        {
        }
        [AllowAnonymous]
        public IActionResult Login(string ReturnUrl)
        {
            return View(new LoginModel { ReturnUrl = ReturnUrl });
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var apiUrl = _options.ApiUrl + "/Account/login";
            await HttpContext.SignOutAsync();

            var loginRes = await _httpClient.PostAsJsonAsync(apiUrl, loginModel);
            if(loginRes.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //
                var res = await _httpClient.GetStringAsync(apiUrl);
                var user = JsonConvert.DeserializeObject<AppUser>(res);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, "AppUser")
                };

                var userIdentity = new ClaimsIdentity(claims, "userLogin");

                var userPrincipal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync("Cookie", userPrincipal,
                    new AuthenticationProperties {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                        IsPersistent = false,
                        AllowRefresh = false
                    });

                return Redirect(loginModel.ReturnUrl ?? "/Todo/Index");
            }
            ModelState.AddModelError("", "Failed to login user");
            return View(loginModel);
        }
    }
}
