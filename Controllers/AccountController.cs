using IdentityUserManagement.Data;
using IdentityUserManagement.Models;
using IdentityUserManagement.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityUserManagement.Controllers
{
    public class AccountController : Controller
    {
        public readonly ApplicationDbContext _dbContext;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;

        public AccountController(ApplicationDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        public IActionResult Index()
        {
            ListAll listAll = new ListAll()
            {
                UserList = _userManager.Users,
                UserRoles = _roleManager.Roles

            };
            return View(listAll);
        }

        public  IActionResult addDeleteClaim()
        {
            string? userId = User.FindFirstValue( ClaimTypes.NameIdentifier);
            var user = _userManager.Users.FirstOrDefault(a=>a.Id== userId);
            var existingUserClaims =  _userManager.GetClaimsAsync(user).Result;
            //var result =  _userManager.RemoveClaimsAsync(user, existingUserClaims).Result;
            var addResult =  _userManager.AddClaimAsync(user, new Claim("Delete", "Delete")).Result;
            return View();
        }

        public IActionResult addCreateClaim()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userManager.Users.FirstOrDefault(a => a.Id == userId);
            var existingUserClaims = _userManager.GetClaimsAsync(user).Result;
            //var result = _userManager.RemoveClaimsAsync(user, existingUserClaims).Result;
            var addResult = _userManager.AddClaimAsync(user, new Claim("Create", "Create")).Result;
            return View();
        }
        [Authorize(Policy = "DeletePolicy")]
        public IActionResult OnlyDelete ()
        {
            return View();
        }

        [Authorize(Policy = "AllPolicy")]
        public IActionResult OnlyAll()
        {
            return View();
        }
        public IActionResult DeleteAllClaims()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userManager.Users.FirstOrDefault(a => a.Id == userId);
            var existingUserClaims = _userManager.GetClaimsAsync(user).Result;
            var result = _userManager.RemoveClaimsAsync(user, existingUserClaims).Result;
            return View("Index");
        }
        public IActionResult Login()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(vm.Email, vm.Password, vm.RememberMe, false);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid Login Attempt!");
            }
            return View(vm);
        }
        public async Task<IActionResult> Register()
        {
            if (!_roleManager.RoleExistsAsync(Utility.Helper.Admin).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(Utility.Helper.Admin));
                await _roleManager.CreateAsync(new IdentityRole(Utility.Helper.Teacher));
                await _roleManager.CreateAsync(new IdentityRole(Utility.Helper.Student));
            }
            return View();
        }
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM m)
        {
            if (ModelState.IsValid)
            {
                var _user = new ApplicationUser
                {
                    Email = m.Email,
                    Name = m.Name,
                    UserName = m.Email
                };
                try
                {
                    var result = _userManager.CreateAsync(_user, m.Password).Result;
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(_user, m.RoleName);
                        await _signInManager.SignInAsync(_user, isPersistent: false);

                        return RedirectToAction("Index", "Home");
                    }
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
                catch(Exception e)
                {
                    return NotFound(e);
                }

            }
            return View(m);
        }
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
