using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected readonly AppDbContext _appDbContext;
        protected readonly UserManager<AppUser> _userManager;
        protected readonly SignInManager<AppUser> _signInManager;
        protected readonly RoleManager<IdentityRole> _roleManager;

        public BaseController(AppDbContext appDbContext){
            _appDbContext = appDbContext;
        }
        public BaseController()
        {

        }
        public BaseController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }
    }

}
